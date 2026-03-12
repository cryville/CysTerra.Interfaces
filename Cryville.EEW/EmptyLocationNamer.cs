using System.Globalization;

namespace Cryville.EEW {
	/// <summary>
	/// An empty <see cref="ILocationNamer" />.
	/// </summary>
	public class EmptyLocationNamer : ILocationNamer {
		static EmptyLocationNamer? s_instance;
		/// <summary>
		/// The shared instance of the <see cref="EmptyLocationNamer" /> class.
		/// </summary>
		public static EmptyLocationNamer Instance => s_instance ??= new();

		/// <inheritdoc />
		public string? Name(double lat, double lon, ref CultureInfo culture, ref int specificity) {
			return null;
		}
	}
}
