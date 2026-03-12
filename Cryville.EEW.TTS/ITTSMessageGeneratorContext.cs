using System;
using System.Globalization;

namespace Cryville.EEW.TTS {
	/// <summary>
	/// Represents a context used in TTS message generators.
	/// </summary>
	public interface ITTSMessageGeneratorContext {
		/// <summary>
		/// The delay tolerance before a nowcast warning event cannot trigger sounds and TTS.
		/// </summary>
		TimeSpan NowcastWarningDelayTolerance { get; }

		/// <summary>
		/// Names a location in a culture.
		/// </summary>
		/// <param name="lat">The latitude of the location.</param>
		/// <param name="lon">The longitude of the location.</param>
		/// <param name="localCulture">The local culture supported by the event itself.</param>
		/// <param name="targetCulture">The target culture of the location name. When the method returns, set to the actual culture of the location name.</param>
		/// <param name="name">The location name.</param>
		/// <returns>Whether the name is given by the context. <see langword="false" /> if the generator should provide a local name instead.</returns>
		bool NameLocation(double lat, double lon, CultureInfo? localCulture, ref CultureInfo targetCulture, out string? name);
	}
}
