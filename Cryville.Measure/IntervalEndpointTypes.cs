using System;

namespace Cryville.Measure {
	/// <summary>
	/// Types of the endpoints of the interval.
	/// </summary>
	[Flags]
	public enum IntervalEndpointTypes {
		/// <summary>
		/// Closed.
		/// </summary>
		None = 0,
		/// <summary>
		/// Left-open.
		/// </summary>
		LeftOpen = 1,
		/// <summary>
		/// Right-open.
		/// </summary>
		RightOpen = 2,
		/// <summary>
		/// Open.
		/// </summary>
		Open = 3,
	}
}
