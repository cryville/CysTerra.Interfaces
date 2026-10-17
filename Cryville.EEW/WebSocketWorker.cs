using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Cryville.EEW {
	/// <summary>
	/// A source worker that pulls events with WebSocket.
	/// </summary>
	public abstract class WebSocketWorker(Uri uri) : IDisposable {
		[SuppressMessage("CodeQuality", "IDE0079", Justification = "False report")]
		[SuppressMessage("Usage", "CA2213", Justification = "Not owned: disposed in the RunAsync task")]
		ClientWebSocket? _socket;

		/// <summary>
		/// The initial URI.
		/// </summary>
		protected Uri InitialUri => uri;
		/// <summary>
		/// Gets the URI of the next connection.
		/// </summary>
		/// <returns>The URI of the next connection.</returns>
		/// <remarks>
		/// If not overridden, the request URI is always the initial URI provided in the constructor.
		/// </remarks>
		protected virtual Uri GetUri() => InitialUri;

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
					_socket = socket;
					string? ua = WebUtils.ToUserAgent(GetType());
					socket.Options.SetRequestHeader("User-Agent", string.IsNullOrWhiteSpace(ua) ? SharedSettings.UserAgent : $"{SharedSettings.UserAgent} {ua}");
					using var ctsTimeout = new CancellationTokenSource(10000);
					var ctTimeout = ctsTimeout.Token;
					try {
						using var ctsLinked = CancellationTokenSource.CreateLinkedTokenSource(ctTimeout, cancellationToken);
						await socket.ConnectAsync(GetUri(), ctsLinked.Token).ConfigureAwait(true);
						reconnectTimeout = 1000;
						OnHeartbeat();
						await OnConnected(cancellationToken).ConfigureAwait(true);
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
#pragma warning disable CS0618
								await Handle(stream, cancellationToken).ConfigureAwait(true);
#pragma warning restore CS0618
								await Handle(stream, result.MessageType, cancellationToken).ConfigureAwait(true);
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
		/// Called when the WebSocket is connected.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>The task.</returns>
		protected virtual Task OnConnected(CancellationToken cancellationToken) => Task.CompletedTask;
		/// <summary>
		/// Handles a response.
		/// </summary>
		/// <param name="stream">The stream that contains the content of the response.</param>
		/// <param name="messageType">The message type.</param>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>The task.</returns>
		protected virtual Task Handle(Stream stream, WebSocketMessageType messageType, CancellationToken cancellationToken) => Task.CompletedTask;
		/// <summary>
		/// Handles a response.
		/// </summary>
		/// <param name="stream">The stream that contains the content of the response.</param>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>The task.</returns>
		[Obsolete("Override Handle(Stream, WebSocketMessageType, CancellationToken) instead.")]
		protected virtual Task Handle(Stream stream, CancellationToken cancellationToken) => Task.CompletedTask;

		readonly SemaphoreSlim _sendSemaphore = new(1, 1);
		/// <summary>
		/// Sends a message.
		/// </summary>
		/// <param name="content">The message content.</param>
		/// <param name="messageType">The message type.</param>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>The task.</returns>
		/// <exception cref="InvalidOperationException">The socket is not connected.</exception>
		protected async Task SendAsync(ReadOnlyMemory<byte> content, WebSocketMessageType messageType, CancellationToken cancellationToken) {
			if (_socket == null)
				throw new InvalidOperationException("Trying to send data while the socket is not connected.");
			await _sendSemaphore.WaitAsync(cancellationToken).ConfigureAwait(true);
			try {
				await _socket.SendAsync(content, messageType, true, cancellationToken).ConfigureAwait(true);
			}
			finally {
				_sendSemaphore.Release();
			}
		}
		/// <summary>
		/// Sends a message.
		/// </summary>
		/// <param name="stream">The stream that contains the content of the message.</param>
		/// <param name="messageType">The message type.</param>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>The task.</returns>
		/// <exception cref="InvalidOperationException">The socket is not connected.</exception>
		protected async Task SendAsync(Stream stream, WebSocketMessageType messageType, CancellationToken cancellationToken) {
			if (_socket == null)
				throw new InvalidOperationException("Trying to send data while the socket is not connected.");
			await _sendSemaphore.WaitAsync(cancellationToken).ConfigureAwait(true);
			try {
				byte[] buffer = ArrayPool<byte>.Shared.Rent(1024);
				while (true) {
					int readCount = await stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(true);
					if (readCount == 0)
						break;
					await _socket.SendAsync(buffer.AsMemory()[..readCount], messageType, true, cancellationToken).ConfigureAwait(true);
				}
			}
			finally {
				_sendSemaphore.Release();
			}
		}

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
