using System;

namespace Cryville.EEW {
	/// <summary>
	/// Specifies retry strategies under certain HTTP responses.
	/// </summary>
	[Flags]
	public enum HttpRetryStrategies {
		/// <summary>
		/// Accepts the response.
		/// </summary>
		None = 0,
		/// <summary>
		/// Reports the response as an error without throwing an exception.
		/// </summary>
		Report = 1,
		/// <summary>
		/// Throws an exception.
		/// </summary>
		Throw = 2,
		/// <summary>
		/// A mask covering the actions on exit.
		/// </summary>
		ExitActionMask = 3,
		/// <summary>
		/// Retries the request before exiting.
		/// </summary>
		RetryBeforeExit = 4,
	}
}
