using Cryville.Common.Compat;
using System;

namespace Cryville.Measure {
	/// <summary>
	/// Quantity with uncertainty.
	/// </summary>
	/// <param name="Value">The value.</param>
	/// <param name="LowerUncertainty">The lower uncertainty.</param>
	/// <param name="UpperUncertainty">The upper uncertainty.</param>
	/// <param name="Unit">The unit.</param>
	public readonly record struct QuantityUnc(double Value, double LowerUncertainty, double UpperUncertainty, Unit Unit) : IInterval, IComparable<QuantityUnc> {
		/// <inheritdoc cref="IInterval.MinValue" />
		public Quantity MinValue => new(Value - LowerUncertainty, Unit);
		/// <inheritdoc cref="IInterval.MaxValue" />
		public Quantity MaxValue => new(Value + UpperUncertainty, Unit);

		object IInterval.MinValue => MinValue;
		object IInterval.MaxValue => MaxValue;
		IntervalEndpointTypes IInterval.EndpointTypes => IntervalEndpointTypes.None;

		/// <summary>
		/// Creates an instance of the <see cref="QuantityUnc" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="uncertainty">The lower and upper uncertainty.</param>
		/// <param name="unit">The unit.</param>
		public QuantityUnc(float value, float uncertainty, Unit unit) : this(Quantity.ToDoubleLiteral(value), Quantity.ToDoubleLiteral(uncertainty), unit) { }
		/// <summary>
		/// Creates an instance of the <see cref="QuantityUnc" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="lowerUncertainty">The lower uncertainty.</param>
		/// <param name="upperUncertainty">The upper uncertainty.</param>
		/// <param name="unit">The unit.</param>
		public QuantityUnc(float value, float lowerUncertainty, float upperUncertainty, Unit unit) : this(Quantity.ToDoubleLiteral(value), Quantity.ToDoubleLiteral(lowerUncertainty), Quantity.ToDoubleLiteral(upperUncertainty), unit) { }
		/// <summary>
		/// Creates an instance of the <see cref="QuantityUnc" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="uncertainty">The lower and upper uncertainty.</param>
		/// <param name="unit">The unit.</param>
		public QuantityUnc(double value, double uncertainty, Unit unit) : this(value, uncertainty, uncertainty, unit) { }
		/// <summary>
		/// Converts the current quantity to a quantity with the specified unit.
		/// </summary>
		/// <param name="unit">The target unit.</param>
		/// <returns>A new quantity converted to <paramref name="unit" />.</returns>
		/// <exception cref="InvalidOperationException">The target unit has a different dimension from the current quantity's unit.</exception>
		public readonly QuantityUnc To(Unit unit) {
			ThrowHelper.ThrowIfNull(unit);
			if (unit == Unit) return this;
			if (Unit.Dimension != unit.Dimension) throw new InvalidOperationException("Dimension mismatched.");
			double value = Unit.To(Value, unit);
			return new(value, value - Unit.To(Value - LowerUncertainty, unit), Unit.To(Value + UpperUncertainty, unit) - value, unit);
		}
		/// <inheritdoc />
		public override readonly string ToString() => Value + "(-" + LowerUncertainty + "/+" + UpperUncertainty + ") (" + Unit + ")";

		/// <inheritdoc />
		public readonly int CompareTo(QuantityUnc other) {
			if (Unit.Dimension != other.Unit.Dimension) throw new InvalidOperationException("Dimension mismatched.");
			return Unit.ToCoherent(Value).CompareTo(other.Unit.ToCoherent(other.Value));
		}
		/// <inheritdoc />
		public readonly bool Equals(QuantityUnc other) {
			if (Unit.Dimension != other.Unit.Dimension) return false;
			return Unit.ToCoherent(Value) == other.Unit.ToCoherent(other.Value)
				&& Unit.ToCoherent(LowerUncertainty) == other.Unit.ToCoherent(other.LowerUncertainty)
				&& Unit.ToCoherent(UpperUncertainty) == other.Unit.ToCoherent(other.UpperUncertainty);
		}
		/// <inheritdoc />
		public override readonly int GetHashCode() => HashCode.Combine(Unit.ToCoherent(Value), Unit.ToCoherent(LowerUncertainty), Unit.ToCoherent(UpperUncertainty), Unit.Dimension);
		/// <inheritdoc />
		public static bool operator <(QuantityUnc left, QuantityUnc right) => left.CompareTo(right) < 0;
		/// <inheritdoc />
		public static bool operator <=(QuantityUnc left, QuantityUnc right) => left.CompareTo(right) <= 0;
		/// <inheritdoc />
		public static bool operator >(QuantityUnc left, QuantityUnc right) => left.CompareTo(right) > 0;
		/// <inheritdoc />
		public static bool operator >=(QuantityUnc left, QuantityUnc right) => left.CompareTo(right) >= 0;
	}
}
