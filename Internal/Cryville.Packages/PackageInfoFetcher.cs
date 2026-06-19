using Cryville.Common.Compat;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;

namespace Cryville.Packages {
	public class PackageInfoFetcher : IDisposable {
		static readonly JsonSerializerOptions _jsonSerializerOptions = new() {
			Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		public Uri BaseUri { get; }
		readonly ILocalPackageRepository _localRepo;

		public PackageInfoFetcher(Uri baseUri, ILocalPackageRepository localRepo) {
			BaseUri = baseUri;
			_localRepo = localRepo;
		}
		public void Dispose() {
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing) {
			if (!disposing)
				return;
			_httpClient.Dispose();
		}

		readonly HttpClient _httpClient = new(new HttpClientHandler() {
			AutomaticDecompression = (DecompressionMethods)(-1),
			CheckCertificateRevocationList = true
		}, true);

		PackageInfoCollection? _cachedPackageInfoCollection;
		public async Task<PackageInfoCollection> FetchPackageInfoCollectionAsync(CancellationToken cancellationToken) {
			var uri = new Uri(BaseUri, "packages.json");
			return _cachedPackageInfoCollection ??= await GetAndDeserializeAsync<PackageInfoCollection>(uri, cancellationToken).ConfigureAwait(false);
		}
		readonly ConcurrentDictionary<Uri, PlatformPackageInfo> _cachedPlatformPackageInfos = [];
		public Task<PlatformPackageInfo> FetchPlatformPackageInfoAsync(string package, CancellationToken cancellationToken) {
			return FetchPlatformPackageInfoAsync(package, _localRepo.PlatformName, cancellationToken);
		}
		public async Task<PlatformPackageInfo> FetchPlatformPackageInfoAsync(string package, string platform, CancellationToken cancellationToken) {
			ThrowHelper.ThrowIfNull(package);
			ThrowHelper.ThrowIfNull(platform);
			var uri = new Uri(BaseUri, string.Format(CultureInfo.InvariantCulture, "{0}/{1}/platform.json", package, platform));
			if (_cachedPlatformPackageInfos.TryGetValue(uri, out var cachedInfo))
				return cachedInfo;
			var fullPackageInfo = await GetAndDeserializeAsync<PlatformPackageInfo>(uri, cancellationToken).ConfigureAwait(false);
			_cachedPlatformPackageInfos.TryAdd(uri, fullPackageInfo);
			return fullPackageInfo;
		}
		readonly ConcurrentDictionary<Uri, VersionInfo> _cachedVersionInfos = [];
		public Task<VersionInfo> FetchVersionInfoAsync(string package, string version, CancellationToken cancellationToken) {
			return FetchVersionInfoAsync(package, _localRepo.PlatformName, version, cancellationToken);
		}
		public async Task<VersionInfo> FetchVersionInfoAsync(string package, string platform, string version, CancellationToken cancellationToken) {
			ThrowHelper.ThrowIfNull(package);
			ThrowHelper.ThrowIfNull(platform);
			ThrowHelper.ThrowIfNull(version);
			var uri = GetVersionInfoUri(package, platform, version);
			if (_cachedVersionInfos.TryGetValue(uri, out var cachedInfo))
				return cachedInfo;
			var versionInfo = await GetAndDeserializeAsync<VersionInfo>(uri, cancellationToken).ConfigureAwait(false);
			_cachedVersionInfos.TryAdd(uri, versionInfo);
			return versionInfo;
		}

		public Uri GetVersionInfoUri(string package, string version) => GetVersionInfoUri(package, _localRepo.PlatformName, version);
		public Uri GetVersionInfoUri(string package, string platform, string version) => new(BaseUri, string.Format(CultureInfo.InvariantCulture, "{0}/{1}/{2}/version.json", package, platform, version));

		async Task<T> GetAndDeserializeAsync<T>(Uri uri, CancellationToken cancellationToken) {
			using var response = await _httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
			response.EnsureSuccessStatusCode();
			using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
			return await JsonSerializer.DeserializeAsync<T>(stream, _jsonSerializerOptions, cancellationToken).ConfigureAwait(false)
				?? throw new InvalidOperationException("Server returned null.");
		}

		public void ClearCache() {
			_cachedPackageInfoCollection = null;
			_cachedPlatformPackageInfos.Clear();
			_cachedVersionInfos.Clear();
		}

		public PackageDownloadTask CreatePackageDownloadTask(string package, VersionInfo versionInfo) {
			return CreatePackageDownloadTask(package, _localRepo.PlatformName, versionInfo);
		}
		public PackageDownloadTask CreatePackageDownloadTask(string package, string platform, VersionInfo versionInfo) {
			ThrowHelper.ThrowIfNull(package);
			ThrowHelper.ThrowIfNull(platform);
			ThrowHelper.ThrowIfNull(versionInfo);
			return new(this, _localRepo, package, platform, versionInfo);
		}
	}
}
