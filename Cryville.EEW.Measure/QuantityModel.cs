using System.Diagnostics.CodeAnalysis;

namespace Cryville.EEW.Measure {
	/// <summary>
	/// Quantity model that describes the range and distribution of a specific type of quantity.
	/// </summary>
	[Experimental("CRYVEEW5002")]
	public abstract class QuantityModel {
		/// <summary>
		/// Selects a representative value from an interval.
		/// </summary>
		/// <param name="min">The minimum value of the interval.</param>
		/// <param name="max">The maximum value of the interval.</param>
		/// <param name="value">When the method returns, set to the selected value.</param>
		/// <returns>Whether a value is selected successfully.</returns>
		public abstract bool SelectValueFromInterval(double min, double max, out ValueWithUncertainty value);
	}
	[Experimental("CRYVEEW5002")]
	public static class QuantityModels {
		public static readonly QuantityModel LinearInfinity = new LinearInfinityQuantityModel();
		public static readonly QuantityModel LinearZeroInfinity = new LinearZeroInfinityQuantityModel();
	}
	static class QuantityModelHelpers {
		public static bool Away(double min, double max, bool noMin, bool noMax, out double value, out bool result) {
			if (noMin) {
				if (noMax) {
					value = default;
					result = false;
					return true;
				}
				value = max;
				result = true;
				return true;
			}
			if (noMax) {
				value = min;
				result = true;
				return true;
			}
			value = default;
			result = default;
			return false;
		}
		[Experimental("CRYVEEW5002")]
		public static bool MidAway(double min, double max, bool noMin, bool noMax, out ValueWithUncertainty value) {
			if (Away(min, max, noMin, noMax, out double v, out bool result)) {
				value = new(v);
				return result;
			}
			value = new((min + max) / 2, (max - min) / 2);
			return true;
		}
	}
	[Experimental("CRYVEEW5002")]
	sealed class LinearInfinityQuantityModel : QuantityModel {
		public override bool SelectValueFromInterval(double min, double max, out ValueWithUncertainty value) {
			return QuantityModelHelpers.MidAway(min, max, double.IsNegativeInfinity(min), double.IsPositiveInfinity(max), out value);
		}
	}
	[Experimental("CRYVEEW5002")]
	sealed class LinearZeroInfinityQuantityModel : QuantityModel {
		public override bool SelectValueFromInterval(double min, double max, out ValueWithUncertainty value) {
			return QuantityModelHelpers.MidAway(min, max, min <= 0, double.IsPositiveInfinity(max), out value);
		}
	}
	[Experimental("CRYVEEW5002")]
	public record struct ValueWithUncertainty(double Value, Uncertainty? Uncertainty = null) {
		public ValueWithUncertainty(double value, double uncertainty) : this(value, new Uncertainty(uncertainty)) { }
	}
	[Experimental("CRYVEEW5002")]
	public record struct Uncertainty(double LowerUncertainty, double UpperUncertainty) {
		public Uncertainty(double uncertainty) : this(uncertainty, uncertainty) { }
	}
}
