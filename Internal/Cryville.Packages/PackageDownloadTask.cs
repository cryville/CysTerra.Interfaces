using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cryville.Packages {
	public class PackageDownloadTask : IDisposable {
		readonly PackageInfoFetcher _fetcher;
		readonly ILocalPackageRepository _localRepo;
		readonly string _package;
		readonly string _platform;
		readonly VersionInfo _versionInfo;

		bool _isTasksResolved;
		readonly ObservableCollection<DownloadTask> m_tasks = [];
		public IReadOnlyCollection<DownloadTask> Tasks => m_tasks;

		readonly HttpClient _httpClient = new(new HttpClientHandler() {
			AutomaticDecompression = (DecompressionMethods)(-1),
			CheckCertificateRevocationList = true
		}, true);

		internal PackageDownloadTask(PackageInfoFetcher fetcher, ILocalPackageRepository localRepo, string package, string platform, VersionInfo versionInfo) {
			_fetcher = fetcher;
			_localRepo = localRepo;
			_package = package;
			_platform = platform;
			_versionInfo = versionInfo;
		}
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing) {
			if (!disposing)
				return;
			_httpClient.Dispose();
		}

		public async Task StartAsync(CancellationToken cancellationToken) {
			if (!_isTasksResolved) {
				var tasks = await ResolveTasksAsync(cancellationToken).ConfigureAwait(false);
				foreach (var task in tasks)
					m_tasks.Add(task.Value);
				_isTasksResolved = true;
			}
			await Task.WhenAll(m_tasks
				.Where(t => !t.IsCompletedSuccessfully)
				.Select(t => t.StartAsync(_httpClient, cancellationToken))
			).ConfigureAwait(false);
		}
		async Task<IReadOnlyDictionary<string, DownloadTask>> ResolveTasksAsync(CancellationToken cancellationToken) {
			var result = new Dictionary<string, DownloadTask>();
			await ResolveTasksAsync(_package, _versionInfo, result, cancellationToken).ConfigureAwait(false);
			return result;
		}
		async Task ResolveTasksAsync(string package, VersionInfo versionInfo, Dictionary<string, DownloadTask> result, CancellationToken cancellationToken) {
			if (versionInfo.Resources is not { } resources)
				throw new PackageNotResolvedException(package, versionInfo.Name, PackageNotResolvedReason.ResourceNotFound);
			if (resources.OfType<FullResourceInfo>().FirstOrDefault() is not { } resource)
				throw new PackageNotResolvedException(package, versionInfo.Name, PackageNotResolvedReason.ResourceNotFound);
			var requiredVersion = new Version(versionInfo.Name);
			lock (result) {
				if (ShouldDownload(package, result, requiredVersion)) {
					result[package] = new(new(_fetcher.GetVersionInfoUri(package, versionInfo.Name), resource.Url), requiredVersion, package, _localRepo);
				}
			}
			if (versionInfo.Dependencies is not { } dependencies)
				return;
			var pendingDependencies = new List<DependencyInfo>();
			var packedDependecies = versionInfo.PackedDependecies ?? new HashSet<string>();
			lock (result) {
				foreach (var d in dependencies) {
					string dependencyId = d.Id;
					if (!ShouldDownload(dependencyId, result, new(d.Version)))
						continue;
					if (packedDependecies.Contains(dependencyId))
						continue;
					pendingDependencies.Add(d);
				}
			}
			await Task.WhenAll(pendingDependencies.Select(d => ResolveDependencyUrisAsync(d, result, cancellationToken))).ConfigureAwait(false);
		}
		bool ShouldDownload(string package, Dictionary<string, DownloadTask> result, Version requiredVersion) =>
			(!_localRepo.TryGetLocalPackageVersion(package, out var installedVersion) || (installedVersion != null ? new Version(installedVersion) : null) < requiredVersion) &&
			(!result.TryGetValue(package, out var existingTask) || existingTask.Version < requiredVersion);
		async Task ResolveDependencyUrisAsync(DependencyInfo dependencyInfo, Dictionary<string, DownloadTask> result, CancellationToken cancellationToken) {
			string package = dependencyInfo.Id;
			string version = dependencyInfo.Version;
			VersionInfo versionInfo;
			try {
				versionInfo = await _fetcher.FetchVersionInfoAsync(package, _platform, version, cancellationToken).ConfigureAwait(false);
			}
			catch (Exception ex) {
				throw new PackageNotResolvedException(package, version, PackageNotResolvedReason.VersionInfoNotFound, ex);
			}
			await ResolveTasksAsync(package, versionInfo, result, cancellationToken).ConfigureAwait(false);
		}
	}
	public sealed record DownloadTask(Uri Uri, Version Version, string FileName, ILocalPackageRepository LocalRepo) : INotifyPropertyChanged {
		public string DownloadFilePath => LocalRepo.GetTempFilePath(FileName);

		public HttpProgress Progress { get; private set; }

		static readonly PropertyChangedEventArgs _Progress_PropertyChangedEventArgs = new(nameof(Progress));
		public event PropertyChangedEventHandler? PropertyChanged;

		public bool IsCompletedSuccessfully { get; private set; }

		internal async Task StartAsync(HttpClient httpClient, CancellationToken cancellationToken) {
			if (IsCompletedSuccessfully)
				return;

			using var response = await httpClient.GetAsync(Uri, cancellationToken).ConfigureAwait(false);
			response.EnsureSuccessStatusCode();
			using var content = response.Content;
			ulong? contentLength = (ulong?)content.Headers.ContentLength;
			using var stream = await content.ReadAsStreamAsync(
#if NET5_0_OR_GREATER
				cancellationToken
#endif
			).ConfigureAwait(false);

			using var outStream = new FileStream(DownloadFilePath, FileMode.Create, FileAccess.Write);
			int bufferLength = 81920;
			if (contentLength != null && contentLength.Value < (ulong)bufferLength)
				bufferLength = (int)contentLength.Value;
			ulong transferredSize = 0;
			Progress = new(0, contentLength);
			byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferLength);
			try {
				while (true) {
					int readLength = await stream.ReadAsync(new Memory<byte>(buffer), cancellationToken).ConfigureAwait(false);
					if (readLength == 0)
						break;
					transferredSize += (ulong)readLength;
					Progress = new(transferredSize, contentLength);
					PropertyChanged?.Invoke(this, _Progress_PropertyChangedEventArgs);
					await outStream.WriteAsync(new ReadOnlyMemory<byte>(buffer, 0, readLength), cancellationToken).ConfigureAwait(false);
				}
			}
			finally {
				ArrayPool<byte>.Shared.Return(buffer);
			}
			IsCompletedSuccessfully = true;
		}
	}
	public readonly record struct HttpProgress(ulong TransferredSize, ulong? TotalSize) {
		public ulong? RemainingSize => TotalSize - TransferredSize;
		public float? Percentage => (float)TransferredSize / TotalSize;
		public bool IsIndeterminate => TotalSize == null;
	}
}
