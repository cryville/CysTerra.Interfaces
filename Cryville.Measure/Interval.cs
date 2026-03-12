using System;
using System.Globalization;

namespace Cryville.Measure {
	/// <summary>
	/// Interval.
	/// </summary>
	/// <typeparam name="T">The value type of the interval.</typeparam>
	/// <param name="MinValue">The minimum value.</param>
	/// <param name="MaxValue">The maximum value.</param>
	/// <param name="EndpointTypes">The type of the endpoints of the interval.</param>
	public readonly record struct Interval<T>(T MinValue, T MaxValue, IntervalEndpointTypes EndpointTypes = IntervalEndpointTypes.None) : IInterval where T : IComparable<T> {
		object IInterval.MinValue => MinValue;
		object IInterval.MaxValue => MaxValue;
		IntervalEndpointTypes IInterval.EndpointTypes => EndpointTypes;

		/// <summary>
		/// Determines whether a quantity is included in the quantity interval.
		/// </summary>
		/// <param name="value">The quantity.</param>
		/// <returns>Whether the specified quantity is included in the quantity interval.</returns>
		public bool Includes(T value) {
			int cMin = value.CompareTo(MinValue);
			int cMax = value.CompareTo(MaxValue);
			return
				(cMin > 0 || cMin == 0 && (EndpointTypes & IntervalEndpointTypes.LeftOpen) == 0) &&
				(cMax < 0 || cMax == 0 && (EndpointTypes & IntervalEndpointTypes.RightOpen) == 0);
		}
		/// <inheritdoc />
		public override string ToString() => string.Format(
			CultureInfo.InvariantCulture, "{0}{1}, {2}{3}",
			(EndpointTypes & IntervalEndpointTypes.LeftOpen) == 0 ? '[' : '(',
			MinValue,
			MaxValue,
			(EndpointTypes & IntervalEndpointTypes.RightOpen) == 0 ? ']' : ')'
		);
	}
}
