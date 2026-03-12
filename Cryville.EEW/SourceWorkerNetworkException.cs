using System;

namespace Cryville.EEW {
	/// <summary>
	/// Thrown when a network error occurs in a source worker.
	/// </summary>
	/// <remarks>
	/// When this exception is thrown, the source worker is considered disconnected until <see cref="ISourceWorker.Heartbeat" /> is raised.
	/// </remarks>
	public class SourceWorkerNetworkException : Exception {
		/// <inheritdoc />
		public SourceWorkerNetworkException() { }
		/// <inheritdoc />
		public SourceWorkerNetworkException(string message) : base(message) { }
		/// <inheritdoc />
		public SourceWorkerNetworkException(string message, Exception innerException) : base(message, innerException) { }
	}
}
