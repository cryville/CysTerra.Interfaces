using Cryville.Common.Compat;
using Cryville.Measure;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Cryville.EEW.Measure {
	[Experimental("CRYVEEW5002")]
	public static class QuantityHelpers {
		static bool TryExtractValueFromInterval(QuantityModel model, double min, double max, out ValueWithUncertainty valueUnc, out int cmp) {
			bool result = model.SelectValueFromInterval(min, max, out valueUnc);
			double value = valueUnc.Value;
			if (!result) {
				cmp = default;
				return false;
			}
			if (value == min) {
				if (value == max) {
					cmp = 0;
					return true;
				}
				cmp = -1;
				return true;
			}
			if (value == max) {
				cmp = 1;
				return true;
			}
			cmp = 0;
			return true;
		}
		static bool TryExtractValueCore(object? v, QuantityModel model, out ValueWithUncertainty valueUnc, out int cmp) {
			try {
				switch (v) {
					case IConvertible convertible:
						valueUnc = new(convertible.ToDouble(CultureInfo.InvariantCulture));
						cmp = 0;
						return true;
					case Interval<float> interval:
						return TryExtractValueFromInterval(model, interval.MinValue, interval.MaxValue, out valueUnc, out cmp); // TODO ToDoubleLiteral
					case Interval<double> interval:
						return TryExtractValueFromInterval(model, interval.MinValue, interval.MaxValue, out valueUnc, out cmp);
					case IInterval interval:
						if (interval.MinValue is IConvertible min && interval.MaxValue is IConvertible max) {
							return TryExtractValueFromInterval(model, min.ToDouble(CultureInfo.InvariantCulture), max.ToDouble(CultureInfo.InvariantCulture), out valueUnc, out cmp);
						}
						break;
				}
			}
			catch (FormatException) { }
			catch (InvalidCastException) { }
			valueUnc = default;
			cmp = default;
			return false;
		}
		public static bool TryExtractValue(object? v, QuantityModel model, out double value) => TryExtractValue(v, model, out value, out _);
		public static bool TryExtractValue(object? v, QuantityModel model, out double value, out int cmp) {
			ThrowHelper.ThrowIfNull(model);
			bool result = TryExtractValueCore(v, model, out var valueUnc, out cmp);
			value = valueUnc.Value;
			return result;
		}
		public static bool TryExtractQuantityValue(object? v, Unit canonicalUnit, QuantityModel model, out double value) {
			bool result = TryExtractQuantityValue(v, canonicalUnit, model, out var valueUnc, out _);
			value = valueUnc.Value;
			return result;
		}
		public static bool TryExtractQuantityValue(object? v, Unit canonicalUnit, QuantityModel model, out ValueWithUncertainty valueUnc, out int cmp) {
			ThrowHelper.ThrowIfNull(model);
			if (TryExtractValueCore(v, model, out valueUnc, out cmp))
				return true;
			switch (v) {
				case Quantity quantity:
					valueUnc = new(quantity.Unit.To(quantity.Value, canonicalUnit));
					cmp = 0;
					return true;
				case QuantityInc quantity:
					var qInc = quantity.To(canonicalUnit);
					return TryExtractValueFromInterval(model, qInc.MinValue.Value, qInc.MaxValue.Value, out valueUnc, out cmp);
				case QuantityUnc quantity:
					var qUnc = quantity.To(canonicalUnit);
					return TryExtractValueFromInterval(model, qUnc.MinValue.Value, qUnc.MaxValue.Value, out valueUnc, out cmp);
				case Interval<Quantity> interval:
					return TryExtractValueFromInterval(model, interval.MinValue.To(canonicalUnit).Value, interval.MaxValue.To(canonicalUnit).Value, out valueUnc, out cmp);
				case Interval<QuantityInc> interval:
					return TryExtractValueFromInterval(model, interval.MinValue.To(canonicalUnit).Value, interval.MaxValue.To(canonicalUnit).Value, out valueUnc, out cmp);
				case Interval<QuantityUnc> interval:
					return TryExtractValueFromInterval(model, interval.MinValue.To(canonicalUnit).Value, interval.MaxValue.To(canonicalUnit).Value, out valueUnc, out cmp);
				case IInterval interval:
					if (interval.MinValue is Quantity min && interval.MaxValue is Quantity max) {
						return TryExtractValueFromInterval(model, min.To(canonicalUnit).Value, max.To(canonicalUnit).Value, out valueUnc, out cmp);
					}
					break;
			}
			valueUnc = default;
			cmp = default;
			return false;
		}
	}
}
