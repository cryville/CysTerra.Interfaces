using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Cryville.EEW {
	/// <summary>
	/// A source worker that pulls events with WebSocket.
	/// </summary>
	public abstract class WebSocketWorker(Uri uri) : IDisposable {
		/// <inheritdoc />
		protected virtual void Dispose(bool disposing) { }
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
		public async Task RunAsync(CancellationToken cancellationToken) {
			try {
				int reconnectTimeout = 1000;
				for (; ; ) {
					using var socket = new ClientWebSocket();
					string? ua = WebUtils.ToUserAgent(GetType());
					socket.Options.SetRequestHeader("User-Agent", string.IsNullOrWhiteSpace(ua) ? SharedSettings.UserAgent : $"{SharedSettings.UserAgent} {ua}");
					using var ctsTimeout = new CancellationTokenSource(10000);
					var ctTimeout = ctsTimeout.Token;
					try {
						using var ctsLinked = CancellationTokenSource.CreateLinkedTokenSource(ctTimeout, cancellationToken);
						await socket.ConnectAsync(uri, ctsLinked.Token).ConfigureAwait(true);
						reconnectTimeout = 1000;
						OnHeartbeat();
						using var stream = new MemoryStream();
						Memory<byte> memory = new(new byte[1024]);
						for (; ; ) {
							var result = await socket.ReceiveAsync(memory, cancellationToken).ConfigureAwait(true);
							stream.Write(memory.Span[..result.Count]);
							if (result.MessageType == WebSocketMessageType.Close) {
								OnError(new SourceWorkerNetworkException("Server closed."));
								goto reconnect;
							}
							if (result.EndOfMessage) {
								stream.Seek(0, SeekOrigin.Begin);
								await Handle(stream, cancellationToken).ConfigureAwait(true);
								stream.SetLength(0);
							}
						}
					}
					catch (WebSocketException ex) {
						OnError(new SourceWorkerNetworkException("Disconnected from server.", ex));
					}
					catch (SourceWorkerNetworkException ex) {
						OnError(ex);
					}
					catch (OperationCanceledException ex) {
						if (cancellationToken.IsCancellationRequested) {
							break;
						}
						else if (ctTimeout.IsCancellationRequested) {
							OnError(new SourceWorkerNetworkException("Connection timed out.", ex));
							goto reconnect;
						}
						else throw;
					}
					finally {
						if (socket.State == WebSocketState.Open) {
							await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, cancellationToken).ConfigureAwait(true);
						}
					}
				reconnect:
					await Task.Delay(reconnectTimeout, cancellationToken).ConfigureAwait(true);
					reconnectTimeout *= 2;
					if (reconnectTimeout > 30000) reconnectTimeout = 30000;
				}
			}
			catch (OperationCanceledException) {
				if (!cancellationToken.IsCancellationRequested) {
					throw;
				}
			}
			catch (Exception) {
				throw;
			}
		}

		/// <summary>
		/// Handles a response.
		/// </summary>
		/// <param name="stream">The stream that contains the content of the response.</param>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>The task.</returns>
		protected abstract Task Handle(Stream stream, CancellationToken cancellationToken);
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
