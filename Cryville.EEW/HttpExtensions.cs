using Cryville.Common.Compat;
using System.Net;
using System.Net.Http;

namespace Cryville.EEW {
	/// <summary>
	/// Provides a set of <see langword="static" /> methods related to HTTP.
	/// </summary>
	public static class HttpExtensions {
		/// <summary>
		/// Throws an exception if the HTTP response has an unaccepted error status code.
		/// </summary>
		/// <param name="response">The response message.</param>
		/// <param name="acceptsClientError">Whether to accept responses with a client error status code (400~499).</param>
		/// <param name="acceptsServerError">Whether to accept responses with a server error status code (500~599).</param>
		/// <returns>The response message if there is no unaccepted error.</returns>
		/// <exception cref="HttpResponseStatusException">The response has an unaccepted error status code.</exception>
		public static HttpResponseMessage EnsureNonErrorStatusCode(this HttpResponseMessage response, bool acceptsClientError = false, bool acceptsServerError = false) {
			ThrowHelper.ThrowIfNull(response);
			if (response.StatusCode is >= (HttpStatusCode)400 and <= (HttpStatusCode)499) {
				if (!acceptsClientError) {
					throw new HttpResponseStatusException(response.StatusCode);
				}
			}
			if (response.StatusCode is >= (HttpStatusCode)500 and <= (HttpStatusCode)599) {
				if (!acceptsServerError) {
					throw new HttpResponseStatusException(response.StatusCode);
				}
			}
			return response;
		}
	}
}
