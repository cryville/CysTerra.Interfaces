using System.Globalization;

namespace Cryville.EEW {
	/// <summary>
	/// Represents a namer that names a location in a specified culture.
	/// </summary>
	public interface ILocationNamer {
		/// <summary>
		/// Names a location in a specified culture.
		/// </summary>
		/// <param name="lat">The latitude.</param>
		/// <param name="lon">The longitude.</param>
		/// <param name="culture">The preferred culture of the name. When the method returns, set to the actual culture of the name.</param>
		/// <param name="specificity">The preferred specificity of the name. When the method returns, set to the actual specificity of the name.</param>
		/// <returns>The name.</returns>
		string? Name(double lat, double lon, ref CultureInfo culture, ref int specificity);
	}
}
