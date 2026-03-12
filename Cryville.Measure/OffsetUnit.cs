namespace Cryville.Measure {
	/// <summary>
	/// Offset unit.
	/// </summary>
	/// <param name="Dimension">The dimension.</param>
	/// <param name="Scale">The scale.</param>
	/// <param name="Offset">The offset.</param>
	/// <remarks>
	/// <paramref name="Scale" /> and <paramref name="Offset" /> define the mapping to the coherent SI unit of the dimension. Given the value in the current unit x, the scale k, and the offset b, the value in the coherent unit y = k * x + b.
	/// </remarks>
	public record OffsetUnit(Dimension Dimension, double Scale = 1, double Offset = 0) : Unit(Dimension, Scale) {
		/// <inheritdoc />
		public override double ToCoherent(double value) => base.ToCoherent(value) + Offset;
		/// <inheritdoc />
		public override double FromCoherent(double value) => base.FromCoherent(value - Offset);
		/// <inheritdoc />
		public override string ToString() {
			string result = "";
			if (Scale != 1) result += Scale + " ";
			result += Dimension;
			if (Offset != 0) result += " + " + Offset;
			return result;
		}
	}
}
