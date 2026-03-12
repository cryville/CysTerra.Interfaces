using System;

namespace Cryville.Measure {
	/// <summary>
	/// Logarithmic unit.
	/// </summary>
	/// <param name="Dimension">The dimension.</param>
	/// <param name="Scale">The scale.</param>
	/// <param name="LogarithmicScale">The logarithmic scale.</param>
	/// <remarks>
	/// <paramref name="Scale" /> and <paramref name="LogarithmicScale"/> define the mapping to the coherent SI unit of the dimension. Given the value in the current unit x, the scale k, and the logarithmic scale a, the value in the coherent unit y = a * exp(k * x).
	/// </remarks>
	public record LogarithmicUnit(Dimension Dimension, double Scale = 1, double LogarithmicScale = 1) : Unit(Dimension, Scale) {
		/// <inheritdoc />
		public override double ToCoherent(double value) => LogarithmicScale * Math.Exp(base.ToCoherent(value));
		/// <inheritdoc />
		public override double FromCoherent(double value) => base.FromCoherent(Math.Log(value / LogarithmicScale));
		/// <inheritdoc />
		public override string ToString() {
			string result = "";
			if (LogarithmicScale != 1) result += LogarithmicScale + " ";
			result += "exp(";
			if (Scale != 1) result += Scale + " ";
			result += Dimension + ")";
			return result;
		}
	}
}
