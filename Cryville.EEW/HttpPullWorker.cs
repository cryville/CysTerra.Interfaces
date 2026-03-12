using Cryville.Common.Compat;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Cryville.EEW {
	/// <summary>
	/// A source worker that pulls events with HTTP GET requests.
	/// </summary>
	public abstract class HttpPullWorker : IDisposable {
		/// <summary>
		/// The HTTP client that interacts with the source.
		/// </summary>
		protected HttpClient Client { get; } = new(new HttpClientHandler() {
			AutomaticDecompression = (DecompressionMethods)(-1),
			CheckCertificateRevocationList = true
		}, true) {
			Timeout = TimeSpan.FromSeconds(10)
		};

		readonly Uri m_uri;
		/// <summary>
		/// The base URI.
		/// </summary>
		protected Uri BaseUri => m_uri;

		/// <summary>
		/// Whether to force the pulling period to be <see cref="DefaultPeriod" />.
		/// </summary>
		/// <remarks>
		/// If set to <see langword="false" />, the client determines the period from the <c>max-age</c> directive in the <c>Cache-Control</c> header of the response if it exists, and clamps it to be at least <see cref="MinimumPeriod" />. The period falls back to <see cref="DefaultPeriod" /> if the directive is not found.
		/// </remarks>
		protected virtual bool ForceDefaultPeriod => false;

		/// <summary>
		/// The default pulling period.
		/// </summary>
		protected virtual TimeSpan DefaultPeriod => TimeSpan.FromSeconds(60);

		/// <summary>
		/// The minimum pulling period.
		/// </summary>
		protected virtual TimeSpan MinimumPeriod => TimeSpan.FromSeconds(2);

		/// <summary>
		/// Creates an instance of the <see cref="HttpPullWorker" /> class.
		/// </summary>
		/// <param name="uri">The base URI of the source.</param>
		protected HttpPullWorker(Uri uri) {
			m_uri = uri;
			Client.DefaultRequestHeaders.UserAgent.ParseAdd(SharedSettings.UserAgent);
			string? ua = WebUtils.ToUserAgent(GetType());
			if (!string.IsNullOrWhiteSpace(ua)) Client.DefaultRequestHeaders.UserAgent.ParseAdd(ua);
		}
		int _disposed;
		/// <inheritdoc />
		protected virtual void Dispose(bool disposing) {
			if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
			if (disposing) {
				Client.Dispose();
			}
		}
		/// <inheritdoc />
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Runs the worker.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>The task.</returns>
		/// <exception cref="InvalidOperationException">The server responses with an unhandled status code.</exception>
		public async Task RunAsync(CancellationToken cancellationToken) {
			try {
				DateTimeOffset? lastModified = null;
				for (; ; ) {
					var period = DefaultPeriod;
					using var response = await TrySendAsync(
						() => new HttpRequestMessage(HttpMethod.Get, GetUri()) { Headers = { IfModifiedSince = lastModified } },
						true, cancellationToken, HttpRetryStrategies.None, HttpRetryStrategies.None
					).ConfigureAwait(true);
					if (response != null) {
						OnHeartbeat();
						if (response.IsSuccessStatusCode)
							lastModified = response.Content.Headers.LastModified;

						await HandleRawResponse(response, cancellationToken).ConfigureAwait(true);
#if !DEBUG
						if (!ForceDefaultPeriod && response.Headers.CacheControl is CacheControlHeaderValue cc && cc.MaxAge is TimeSpan maxAge) {
							period = maxAge;
						}
#endif
						await AfterHandled(cancellationToken).ConfigureAwait(true);
					}
					if (period < MinimumPeriod)
						period = MinimumPeriod;
					await Task.Delay(period, cancellationToken).ConfigureAwait(true);
				}
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) {
				// Do nothing: Canceled
			}
		}

		/// <summary>
		/// Handles a raw response message.
		/// </summary>
		/// <param name="response">The response message.</param>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>A task.</returns>
		protected virtual async Task HandleRawResponse(HttpResponseMessage response, CancellationToken cancellationToken) {
			ThrowHelper.ThrowIfNull(response);
			try {
				response.EnsureNonErrorStatusCode();
			}
			catch (HttpResponseStatusException ex) {
				if (ex.IsClientError)
					throw;
				OnError(new SourceWorkerNetworkException("Failed to fetch report catalog.", ex));
				return;
			}
			catch (HttpRequestException ex) {
				OnError(new SourceWorkerNetworkException("Failed to fetch report catalog.", ex));
				return;
			}
			if (response.StatusCode != HttpStatusCode.OK)
				return;
			using var stream = await response.Content.ReadAsStreamAsync(
#if NET5_0_OR_GREATER
				cancellationToken
#endif
			).ConfigureAwait(true);
			await Handle(stream, response.Headers, cancellationToken).ConfigureAwait(true);
		}

		/// <summary>
		/// Try to send a GET request to the specified URI.
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <param name="clientErrorRetryStrategies">The retry strategy on client error.</param>
		/// <param name="serverErrorRetryStrategies">The retry strategy on server error.</param>
		/// <param name="retries">Times to retry before the request fails.</param>
		/// <returns>The response message, or <see langword="null" /> if the request fails.</returns>
		protected async Task<HttpResponseMessage?> TryGetAsync(Uri uri, CancellationToken cancellationToken, HttpRetryStrategies clientErrorRetryStrategies = HttpRetryStrategies.Throw, HttpRetryStrategies serverErrorRetryStrategies = HttpRetryStrategies.Report | HttpRetryStrategies.RetryBeforeExit, int retries = 5) {
			return await TrySendAsync(() => new HttpRequestMessage(HttpMethod.Get, uri), cancellationToken, clientErrorRetryStrategies, serverErrorRetryStrategies, retries).ConfigureAwait(true);
		}

		/// <summary>
		/// Try to send a request to the specified URI.
		/// </summary>
		/// <param name="msgFactory">A function that creates the request message.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <param name="clientErrorRetryStrategies">The retry strategy on client error.</param>
		/// <param name="serverErrorRetryStrategies">The retry strategy on server error.</param>
		/// <param name="retries">Times to retry before the request fails.</param>
		/// <returns>The response message, or <see langword="null" /> if the request fails.</returns>
		protected Task<HttpResponseMessage?> TrySendAsync(Func<HttpRequestMessage> msgFactory, CancellationToken cancellationToken, HttpRetryStrategies clientErrorRetryStrategies = HttpRetryStrategies.Throw, HttpRetryStrategies serverErrorRetryStrategies = HttpRetryStrategies.Report | HttpRetryStrategies.RetryBeforeExit, int retries = 5) {
			ThrowHelper.ThrowIfNull(msgFactory);
			return TrySendAsync(msgFactory, false, cancellationToken, clientErrorRetryStrategies, serverErrorRetryStrategies, retries);
		}

		async Task<HttpResponseMessage?> TrySendAsync(Func<HttpRequestMessage> msgFactory, bool isMainRequest, CancellationToken cancellationToken, HttpRetryStrategies clientErrorRetryStrategies = HttpRetryStrategies.Throw, HttpRetryStrategies serverErrorRetryStrategies = HttpRetryStrategies.Report | HttpRetryStrategies.RetryBeforeExit, int retries = 5) {
			Exception? lastEx = null;
			for (int retry = 0; retry < retries; retry++) {
				bool isLastRetry = retry == retries - 1;
				bool acceptsClientError = (clientErrorRetryStrategies & HttpRetryStrategies.ExitActionMask) == HttpRetryStrategies.None;
				bool acceptsServerError = (serverErrorRetryStrategies & HttpRetryStrategies.ExitActionMask) == HttpRetryStrategies.None;
				try {
					using var msg = msgFactory();
					var response = await Client.SendAsync(msg, cancellationToken).ConfigureAwait(true);
					if (!isLastRetry) {
						acceptsClientError &= (clientErrorRetryStrategies & HttpRetryStrategies.RetryBeforeExit) == 0;
						acceptsServerError &= (serverErrorRetryStrategies & HttpRetryStrategies.RetryBeforeExit) == 0;
					}
					return response.EnsureNonErrorStatusCode(acceptsClientError, acceptsServerError);
				}
				catch (HttpResponseStatusException ex) {
					if (ex.IsClientError) {
						if (ShouldThrow(clientErrorRetryStrategies, isLastRetry)) {
							throw;
						}
					}
					else if (ex.IsServerError) {
						if (ShouldThrow(serverErrorRetryStrategies, isLastRetry)) {
							throw;
						}
					}
					static bool ShouldThrow(HttpRetryStrategies errorRetryStrategies, bool isLastRetry) {
						if (!isLastRetry && (errorRetryStrategies & HttpRetryStrategies.RetryBeforeExit) != 0)
							return false;
						return (errorRetryStrategies & HttpRetryStrategies.Throw) != 0;
					}
					lastEx = ex;
				}
				catch (HttpRequestException ex) {
					lastEx = ex;
				}
				catch (WebException ex) {
					lastEx = ex;
				}
				catch (OperationCanceledException ex) when (!cancellationToken.IsCancellationRequested) {
					lastEx = ex;
				}
				await Task.Delay(1000).ConfigureAwait(true);
			}
			if (lastEx != null) OnError(isMainRequest ? new SourceWorkerNetworkException("Failed to fetch report catalog.", lastEx) : lastEx);
			return null;
		}

		/// <summary>
		/// Gets the URI of the next request, usually based on <see cref="BaseUri" />.
		/// </summary>
		/// <returns>The URI of the next request.</returns>
		/// <remarks>
		/// If not overridden, the request URI is always <see cref="BaseUri" />.
		/// </remarks>
		protected virtual Uri GetUri() => m_uri;
		/// <summary>
		/// Called when a response is handled successfully, or when the server responses with a non-error status code (100~399).
		/// </summary>
		protected virtual Task AfterHandled(CancellationToken cancellationToken) => Task.CompletedTask;
		/// <summary>
		/// Handles a response.
		/// </summary>
		/// <param name="stream">The stream that contains the content of the response.</param>
		/// <param name="headers">The response headers.</param>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>The task.</returns>
		protected abstract Task Handle(Stream stream, HttpResponseHeaders headers, CancellationToken cancellationToken);
		/// <summary>
		/// Called when a response is received.
		/// </summary>
		protected abstract void OnHeartbeat();
		/// <summary>
		/// Called when an error occurs.
		/// </summary>
		/// <param name="ex">The exception.</param>
		protected abstract void OnError(Exception ex);
	}
}
