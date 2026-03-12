using System;
using System.Net;
using System.Net.Http;

namespace Cryville.EEW {
	/// <summary>
	/// Thrown when a response with an unexpected status code is received.
	/// </summary>
	public class HttpResponseStatusException : HttpRequestException {
		/// <summary>
		/// Whether the status code represents a client error.
		/// </summary>
		public bool IsClientError => StatusCode is >= (HttpStatusCode)400 and <= (HttpStatusCode)499;
		/// <summary>
		/// Whether the status code represents a server error.
		/// </summary>
		public bool IsServerError => StatusCode is >= (HttpStatusCode)500 and <= (HttpStatusCode)599;

		/// <inheritdoc />
		public HttpResponseStatusException() { }
		/// <inheritdoc />
		public HttpResponseStatusException(string message) : base(message) { }
		/// <inheritdoc />
		public HttpResponseStatusException(string message, Exception innerException) : base(message, innerException) { }
#if NET5_0_OR_GREATER
		/// <inheritdoc />
		public HttpResponseStatusException(string? message, Exception? inner, HttpStatusCode statusCode) : base(message, inner, statusCode) { }
#else
        /// <summary>
        /// Gets the HTTP status code to be returned with the exception.
        /// </summary>
		public HttpStatusCode? StatusCode { get; }
		/// <summary>
		/// Initializes a new instance of the <see cref="HttpResponseStatusException" /> class with a specific message that describes the current exception, an inner exception, and an HTTP status code.
		/// </summary>
		/// <param name="message">A message that describes the current exception.</param>
		/// <param name="inner">The inner exception.</param>
		/// <param name="statusCode">The HTTP status code.</param>
		public HttpResponseStatusException(string? message, Exception? inner, HttpStatusCode? statusCode) : base(message, inner) {
			StatusCode = statusCode;
		}
#endif
		/// <summary>
		/// Initializes a new instance of the <see cref="HttpResponseStatusException" /> class with an HTTP status code.
		/// </summary>
		/// <param name="statusCode">The HTTP status code.</param>
		public HttpResponseStatusException(HttpStatusCode statusCode) : this($"HTTP error: {statusCode}.", null, statusCode) { }
	}
}
