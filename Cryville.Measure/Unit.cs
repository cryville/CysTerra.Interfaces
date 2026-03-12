using Cryville.Common.Compat;
using System;

namespace Cryville.Measure {
	/// <summary>
	/// Unit.
	/// </summary>
	/// <param name="Dimension">The dimension.</param>
	/// <param name="Scale">The scale.</param>
	/// <remarks>
	/// <paramref name="Scale" /> define the mapping to the coherent SI unit of the dimension. Given the value in the current unit x and the scale k, the value in the coherent unit y = k * x.
	/// </remarks>
	public record Unit(Dimension Dimension, double Scale = 1) {
		/// <summary>
		/// Creates a unit based on the current unit modified by the specified prefix.
		/// </summary>
		/// <param name="prefix">The prefix.</param>
		/// <returns>The new unit.</returns>
		public Unit WithPrefix(double prefix) => this with { Scale = prefix * Scale };
		/// <summary>
		/// Converts the specified value in the current unit to a value in the corresponding coherent unit.
		/// </summary>
		/// <param name="value">The value in the current unit.</param>
		/// <returns>A value in the corresponding coherent unit.</returns>
		public virtual double ToCoherent(double value) => Scale * value;
		/// <summary>
		/// Converts the specified value in the coherent unit to a value in the current unit.
		/// </summary>
		/// <param name="value">The value in the coherent unit.</param>
		/// <returns>A value in the current unit.</returns>
		public virtual double FromCoherent(double value) => value / Scale;
		/// <summary>
		/// Converts the specified value in the specified unit to a value in the current unit.
		/// </summary>
		/// <param name="value">The value in <paramref name="unit" />.</param>
		/// <param name="unit">The source unit.</param>
		/// <returns>A value in the current unit.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="unit" /> is <see langword="null" />.</exception>
		public double From(double value, Unit unit) {
			ThrowHelper.ThrowIfNull(unit);
			if (Equals(unit)) return value;
			return FromCoherent(unit.ToCoherent(value));
		}
		/// <summary>
		/// Converts the specified value in the current unit to a value in the specified unit.
		/// </summary>
		/// <param name="value">The value in the current unit.</param>
		/// <param name="unit">The target unit.</param>
		/// <returns>A value in <paramref name="unit" />.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="unit" /> is <see langword="null" />.</exception>
		public double To(double value, Unit unit) {
			ThrowHelper.ThrowIfNull(unit);
			if (Equals(unit)) return value;
			return unit.FromCoherent(ToCoherent(value));
		}
		/// <inheritdoc />
		public override string ToString() {
			string result = "";
			if (Scale != 1) result += Scale + " ";
			result += Dimension;
			return result;
		}
		/// <summary>
		/// Returns the multiplication of two units.
		/// </summary>
		/// <param name="left">The first unit.</param>
		/// <param name="right">The second unit.</param>
		/// <returns>The multiplication of the two units.</returns>
		public static Unit Multiply(Unit left, Unit right) {
			ThrowHelper.ThrowIfNull(left);
			ThrowHelper.ThrowIfNull(right);
			if (left.GetType() != typeof(Unit) || right.GetType() != typeof(Unit)) throw new InvalidOperationException("Cannot multiply non-linear units.");
			return new(left.Dimension * right.Dimension, left.Scale * right.Scale);
		}
		/// <inheritdoc cref="Multiply(Unit, Unit)" />
		public static Unit operator *(Unit left, Unit right) => Multiply(left, right);
		/// <summary>
		/// Returns the division of a unit divided by another.
		/// </summary>
		/// <param name="left">The divided unit.</param>
		/// <param name="right">The dividing unit.</param>
		/// <returns>The division of <paramref name="left" /> divided by <paramref name="right" />.</returns>
		public static Unit Divide(Unit left, Unit right) {
			ThrowHelper.ThrowIfNull(left);
			ThrowHelper.ThrowIfNull(right);
			if (left.GetType() != typeof(Unit) || right.GetType() != typeof(Unit)) throw new InvalidOperationException("Cannot multiply non-linear units.");
			return new(left.Dimension / right.Dimension, left.Scale / right.Scale);
		}
		/// <inheritdoc cref="Divide(Unit, Unit)" />
		public static Unit operator /(Unit left, Unit right) => Divide(left, right);
	}
}
