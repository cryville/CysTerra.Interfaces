using System.Globalization;

namespace Cryville.Measure {
	/// <summary>
	/// Physical dimension.
	/// </summary>
	public record struct Dimension {
		/// <summary>
		/// The dimensions of time.
		/// </summary>
		public sbyte Time { get; set; }
		/// <summary>
		/// The dimensions of length.
		/// </summary>
		public sbyte Length { get; set; }
		/// <summary>
		/// The dimensions of mass.
		/// </summary>
		public sbyte Mass { get; set; }
		/// <summary>
		/// The dimensions of electric current.
		/// </summary>
		public sbyte ElectricCurrent { get; set; }
		/// <summary>
		/// The dimensions of thermodynamic temperature.
		/// </summary>
		public sbyte ThermodynamicTemperature { get; set; }
		/// <summary>
		/// The dimensions of amount of substance.
		/// </summary>
		public sbyte AmountOfSubstance { get; set; }
		/// <summary>
		/// The dimensions of luminous intensity.
		/// </summary>
		public sbyte LuminousIntensity { get; set; }
		/// <inheritdoc />
		public override readonly string ToString() {
			if (Equals(default)) return "1";
			string result = "";
			DimensionToString(ref result, Time, 'T');
			DimensionToString(ref result, Length, 'L');
			DimensionToString(ref result, Mass, 'M');
			DimensionToString(ref result, ElectricCurrent, 'I');
			DimensionToString(ref result, ThermodynamicTemperature, '\x0398');
			DimensionToString(ref result, AmountOfSubstance, 'N');
			DimensionToString(ref result, LuminousIntensity, 'J');
			return result;
		}
		static void DimensionToString(ref string result, sbyte dim, char sym) {
			if (dim == 0) return;
			if (!string.IsNullOrEmpty(result)) result += "\xb7";
			if (dim == 1) result += sym;
			else result += string.Format(CultureInfo.InvariantCulture, dim > 0 ? "{0}^{1}" : "{0}^({1})", sym, dim);
		}
		/// <summary>
		/// Returns the multiplication of two dimensions.
		/// </summary>
		/// <param name="left">The first dimension.</param>
		/// <param name="right">The second dimension.</param>
		/// <returns>The multiplication of the two dimensions.</returns>
		public static Dimension Multiply(Dimension left, Dimension right) => checked(new() {
			Time = (sbyte)(left.Time + right.Time),
			Length = (sbyte)(left.Length + right.Length),
			Mass = (sbyte)(left.Mass + right.Mass),
			ElectricCurrent = (sbyte)(left.ElectricCurrent + right.ElectricCurrent),
			ThermodynamicTemperature = (sbyte)(left.ThermodynamicTemperature + right.ThermodynamicTemperature),
			AmountOfSubstance = (sbyte)(left.AmountOfSubstance + right.AmountOfSubstance),
			LuminousIntensity = (sbyte)(left.LuminousIntensity + right.LuminousIntensity),
		});
		/// <inheritdoc cref="Multiply(Dimension, Dimension)" />
		public static Dimension operator *(Dimension left, Dimension right) => Multiply(left, right);
		/// <summary>
		/// Returns the division of a dimension divided by another.
		/// </summary>
		/// <param name="left">The divided dimension.</param>
		/// <param name="right">The dividing dimension.</param>
		/// <returns>The division of <paramref name="left" /> divided by <paramref name="right" />.</returns>
		public static Dimension Divide(Dimension left, Dimension right) => checked(new() {
			Time = (sbyte)(left.Time - right.Time),
			Length = (sbyte)(left.Length - right.Length),
			Mass = (sbyte)(left.Mass - right.Mass),
			ElectricCurrent = (sbyte)(left.ElectricCurrent - right.ElectricCurrent),
			ThermodynamicTemperature = (sbyte)(left.ThermodynamicTemperature - right.ThermodynamicTemperature),
			AmountOfSubstance = (sbyte)(left.AmountOfSubstance - right.AmountOfSubstance),
			LuminousIntensity = (sbyte)(left.LuminousIntensity - right.LuminousIntensity),
		});
		/// <inheritdoc cref="Divide(Dimension, Dimension)" />
		public static Dimension operator /(Dimension left, Dimension right) => Divide(left, right);
	}
}
