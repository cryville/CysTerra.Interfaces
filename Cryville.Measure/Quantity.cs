using Cryville.Common.Compat;
using System;

namespace Cryville.Measure {
	/// <summary>
	/// Physical quantity.
	/// </summary>
	/// <param name="Value">The value.</param>
	/// <param name="Unit">The unit.</param>
	public record struct Quantity(double Value, Unit Unit) : IComparable<Quantity> {
		/// <summary>
		/// Creates an instance of the <see cref="Quantity" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="unit">The unit.</param>
		public Quantity(float value, Unit unit) : this(ToDoubleLiteral(value), unit) { }
		internal static double ToDoubleLiteral(float value) {
			int exp = (byte)(unchecked((uint)BitConverter.SingleToInt32Bits(value)) >> 23) - 126;
			if (exp > 96) {
				return double.PositiveInfinity;
			}
			return (double)(decimal)value;
		}

		/// <summary>
		/// Converts the current quantity to a quantity with the specified unit.
		/// </summary>
		/// <param name="unit">The target unit.</param>
		/// <returns>A new quantity converted to <paramref name="unit" />.</returns>
		/// <exception cref="InvalidOperationException">The target unit has a different dimension from the current quantity's unit.</exception>
		public readonly Quantity To(Unit unit) {
			ThrowHelper.ThrowIfNull(unit);
			if (unit == Unit) return this;
			if (Unit.Dimension != unit.Dimension) throw new InvalidOperationException("Dimension mismatched.");
			return new(Unit.To(Value, unit), unit);
		}
		readonly double ToCoherentValue() => Unit.ToCoherent(Value);
		/// <inheritdoc />
		public override readonly string ToString() => Value + " (" + Unit + ")";

		/// <inheritdoc />
		public readonly int CompareTo(Quantity other) {
			if (Unit.Dimension != other.Unit.Dimension) throw new InvalidOperationException("Dimension mismatched.");
			return ToCoherentValue().CompareTo(other.ToCoherentValue());
		}
		/// <inheritdoc />
		public readonly bool Equals(Quantity other) {
			if (Unit.Dimension != other.Unit.Dimension) return false;
			return ToCoherentValue() == other.ToCoherentValue();
		}
		/// <inheritdoc />
		public override readonly int GetHashCode() => HashCode.Combine(ToCoherentValue(), Unit.Dimension);
		/// <inheritdoc />
		public static bool operator <(Quantity left, Quantity right) => left.CompareTo(right) < 0;
		/// <inheritdoc />
		public static bool operator <=(Quantity left, Quantity right) => left.CompareTo(right) <= 0;
		/// <inheritdoc />
		public static bool operator >(Quantity left, Quantity right) => left.CompareTo(right) > 0;
		/// <inheritdoc />
		public static bool operator >=(Quantity left, Quantity right) => left.CompareTo(right) >= 0;
	}
}
