using Cryville.Common.Compat;
using System.Globalization;

namespace Cryville.EEW.Report {
	/// <summary>
	/// Represents a context used in report model generators.
	/// </summary>
	public interface IReportGeneratorContext {
		/// <summary>
		/// The severity scheme.
		/// </summary>
		ISeverityScheme SeverityScheme { get; }

		/// <summary>
		/// Names a location in a culture.
		/// </summary>
		/// <param name="lat">The latitude of the location.</param>
		/// <param name="lon">The longitude of the location.</param>
		/// <param name="localCulture">The local culture supported by the event itself.</param>
		/// <param name="targetCulture">The target culture of the location name. When the method returns, set to the actual culture of the location name.</param>
		/// <param name="name">The location name.</param>
		/// <param name="specificity">The location specificity. See <see cref="ReportModel.LocationSpecificity" />.</param>
		/// <returns>Whether the name is given by the context. <see langword="false" /> if the generator should provide a local name instead.</returns>
		bool NameLocation(double lat, double lon, CultureInfo? localCulture, ref CultureInfo targetCulture, out string? name, out int specificity);
	}
	/// <summary>
	/// Provides a set of <see langword="static" /> methods related to <see cref="IReportGeneratorContext" />.
	/// </summary>
	public static class ReportGeneratorContextExtensions {
		/// <summary>
		/// Names a location in a culture and sets the result to a report model.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="e">The report model. Its <see cref="ReportModel.Location" /> and <see cref="ReportModel.LocationSpecificity" /> are modified to the name given by the context, if and only if this method returns <see langword="true" />.</param>
		/// <param name="lat">The latitude of the location.</param>
		/// <param name="lon">The longitude of the location.</param>
		/// <param name="localCulture">The local culture supported by the event itself.</param>
		/// <param name="targetCulture">The target culture of the location name. When the method returns, set to the actual culture of the location name.</param>
		/// <returns>Whether the name is given by the context. <see langword="false" /> if the generator should provide a local name instead.</returns>
		public static bool NameLocationTo(this IReportGeneratorContext context, ReportModel e, double lat, double lon, CultureInfo? localCulture, CultureInfo targetCulture) {
			ThrowHelper.ThrowIfNull(context);
			ThrowHelper.ThrowIfNull(e);
			if (context.NameLocation(lat, lon, localCulture, ref targetCulture, out string? name, out int specificity)) {
				e.Location = name;
				e.LocationSpecificity = specificity;
				return true;
			}
			else {
				return false;
			}
		}
	}
}
