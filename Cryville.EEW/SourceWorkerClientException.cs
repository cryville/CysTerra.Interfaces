using System;

namespace Cryville.EEW {
	/// <summary>
	/// Thrown when an error occurs in a source worker for which the user is considered to be responsible.
	/// </summary>
	public class SourceWorkerClientException : Exception {
		/// <inheritdoc />
		public SourceWorkerClientException() { }
		/// <inheritdoc />
		public SourceWorkerClientException(string message) : base(message) { }
		/// <inheritdoc />
		public SourceWorkerClientException(string message, Exception innerException) : base(message, innerException) { }
	}
}
