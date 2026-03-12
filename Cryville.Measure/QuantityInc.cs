using Cryville.Common.Compat;
using System;

namespace Cryville.Measure {
	/// <summary>
	/// Quantity with implied uncertainty.
	/// </summary>
	/// <param name="Value">The value.</param>
	/// <param name="Uncertainty">The uncertainty.</param>
	/// <param name="Unit">The unit.</param>
	public readonly record struct QuantityInc(double Value, double Uncertainty, Unit Unit) : IInterval, IComparable<QuantityInc> {
		/// <inheritdoc cref="IInterval.MinValue" />
		public Quantity MinValue => new(Value - Uncertainty, Unit);
		/// <inheritdoc cref="IInterval.MaxValue" />
		public Quantity MaxValue => new(Value + Uncertainty, Unit);

		bool IInterval.IsExplicit => false;
		object IInterval.MinValue => MinValue;
		object IInterval.MaxValue => MaxValue;
		IntervalEndpointTypes IInterval.EndpointTypes => IntervalEndpointTypes.None;

		/// <summary>
		/// Creates an instance of the <see cref="QuantityInc" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="unit">The unit.</param>
		public QuantityInc(float value, Unit unit) : this(Quantity.ToDoubleLiteral(value), unit) { }
		/// <summary>
		/// Creates an instance of the <see cref="QuantityInc" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="uncertainty">The lower and upper uncertainty.</param>
		/// <param name="unit">The unit.</param>
		public QuantityInc(float value, float uncertainty, Unit unit) : this(Quantity.ToDoubleLiteral(value), Quantity.ToDoubleLiteral(uncertainty), unit) { }
		/// <summary>
		/// Creates an instance of the <see cref="QuantityInc" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="unit">The unit.</param>
		public QuantityInc(double value, Unit unit) : this(value, InferUncertainty(value), unit) { }
		static double InferUncertainty(double value) {
			if ((((int)(unchecked((ulong)BitConverter.DoubleToInt64Bits(value)) >> 52) & 0x7FF) - 1022) > 96) {
				return 0;
			}
			decimal dec = (decimal)value;
#if NET7_0_OR_GREATER
			byte scale = dec.Scale;
#elif NET5_0_OR_GREATER
			Span<int> buffer = stackalloc int[4];
			decimal.GetBits(dec, buffer);
			byte scale = (byte)(buffer[3] >> 16);
#else
			byte scale = (byte)(decimal.GetBits(dec)[3] >> 16);
#endif
			return Math.Pow(10, -scale) / 2;
		}

		/// <summary>
		/// Converts the current quantity to a quantity with the specified unit.
		/// </summary>
		/// <param name="unit">The target unit.</param>
		/// <returns>A new quantity converted to <paramref name="unit" />.</returns>
		/// <exception cref="InvalidOperationException">The target unit has a different dimension from the current quantity's unit.</exception>
		public readonly QuantityInc To(Unit unit) {
			ThrowHelper.ThrowIfNull(unit);
			if (unit == Unit) return this;
			if (Unit.Dimension != unit.Dimension) throw new InvalidOperationException("Dimension mismatched.");
			double value = Unit.To(Value, unit);
			return new(value, value - Unit.To(Value - Uncertainty, unit), unit);
		}
		/// <inheritdoc />
		public override readonly string ToString() => Value + "±" + Uncertainty + " (" + Unit + ")";

		/// <inheritdoc />
		public readonly int CompareTo(QuantityInc other) {
			if (Unit.Dimension != other.Unit.Dimension) throw new InvalidOperationException("Dimension mismatched.");
			return Unit.ToCoherent(Value).CompareTo(other.Unit.ToCoherent(other.Value));
		}
		/// <inheritdoc />
		public readonly bool Equals(QuantityInc other) {
			if (Unit.Dimension != other.Unit.Dimension) return false;
			return Unit.ToCoherent(Value) == other.Unit.ToCoherent(other.Value)
				&& Unit.ToCoherent(Uncertainty) == other.Unit.ToCoherent(other.Uncertainty);
		}
		/// <inheritdoc />
		public override readonly int GetHashCode() => HashCode.Combine(Unit.ToCoherent(Value), Unit.ToCoherent(Uncertainty), Unit.Dimension);
		/// <inheritdoc />
		public static bool operator <(QuantityInc left, QuantityInc right) => left.CompareTo(right) < 0;
		/// <inheritdoc />
		public static bool operator <=(QuantityInc left, QuantityInc right) => left.CompareTo(right) <= 0;
		/// <inheritdoc />
		public static bool operator >(QuantityInc left, QuantityInc right) => left.CompareTo(right) > 0;
		/// <inheritdoc />
		public static bool operator >=(QuantityInc left, QuantityInc right) => left.CompareTo(right) >= 0;
	}
}
