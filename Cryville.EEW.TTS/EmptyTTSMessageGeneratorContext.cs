using System;
using System.Globalization;

namespace Cryville.EEW.TTS {
	/// <summary>
	/// An empty <see cref="ITTSMessageGeneratorContext" />.
	/// </summary>
	public class EmptyTTSMessageGeneratorContext : ITTSMessageGeneratorContext {
		static EmptyTTSMessageGeneratorContext? s_instance;
		/// <summary>
		/// The shared instance of the <see cref="EmptyTTSMessageGeneratorContext" /> class.
		/// </summary>
		public static EmptyTTSMessageGeneratorContext Instance => s_instance ??= new();

		/// <inheritdoc />
		public TimeSpan NowcastWarningDelayTolerance => TimeSpan.MaxValue;

		/// <inheritdoc />
		public bool NameLocation(double lat, double lon, CultureInfo? localCulture, ref CultureInfo targetCulture, out string? name) {
			name = null;
			return false;
		}
	}
}
