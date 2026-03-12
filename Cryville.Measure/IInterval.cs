namespace Cryville.Measure {
	/// <summary>
	/// Interval.
	/// </summary>
	public interface IInterval {
		/// <summary>
		/// Whether the interval is explicit.
		/// </summary>
		bool IsExplicit => true;
		/// <summary>
		/// The minimum value.
		/// </summary>
		object MinValue { get; }
		/// <summary>
		/// The maximum value.
		/// </summary>
		object MaxValue { get; }
		/// <summary>
		/// The type of the endpoints of the interval.
		/// </summary>
		IntervalEndpointTypes EndpointTypes { get; }
	}
}
