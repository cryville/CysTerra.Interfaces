using System.Globalization;

namespace Cryville.EEW.Report {
	/// <summary>
	/// An empty <see cref="IReportGeneratorContext" />.
	/// </summary>
	public class EmptyReportGeneratorContext : IReportGeneratorContext {
		static EmptyReportGeneratorContext? s_instance;
		/// <summary>
		/// The shared instance of the <see cref="EmptyReportGeneratorContext" /> class.
		/// </summary>
		public static EmptyReportGeneratorContext Instance => s_instance ??= new();

		/// <inheritdoc />
		public ISeverityScheme SeverityScheme => EmptySeverityScheme.Instance;

		/// <inheritdoc />
		public bool NameLocation(double lat, double lon, CultureInfo? localCulture, ref CultureInfo targetCulture, out string? name, out int specificity) {
			name = null;
			specificity = 0;
			return false;
		}
	}
}
