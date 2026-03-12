using Cryville.Measure;
using System.Runtime.CompilerServices;

namespace Cryville.EEW.Measure {
	/// <summary>
	/// Named metric prefixes.
	/// </summary>
	public static class NamedMetricPrefixes {
		static readonly ILocalizableMessageStringSet _res = new LocalizableResource("").RootMessageStringSet;
		static NamedPrefix Create(double scale, string symbolAffix, [CallerMemberName] string nameKey = "") =>
			new(scale, symbolAffix, _res.GetStringOrDefault(nameKey + "Short", nameKey), _res.GetStringRequired(nameKey));

		/// <inheritdoc cref="MetricPrefixes.Tera" />
		public static readonly NamedPrefix Tera = Create(MetricPrefixes.Tera, "T{0}");
		/// <inheritdoc cref="MetricPrefixes.Giga" />
		public static readonly NamedPrefix Giga = Create(MetricPrefixes.Giga, "G{0}");
		/// <inheritdoc cref="MetricPrefixes.Mega" />
		public static readonly NamedPrefix Mega = Create(MetricPrefixes.Mega, "M{0}");
		/// <inheritdoc cref="MetricPrefixes.Kilo" />
		public static readonly NamedPrefix Kilo = Create(MetricPrefixes.Kilo, "k{0}");
		/// <inheritdoc cref="MetricPrefixes.Hecto" />
		public static readonly NamedPrefix Hecto = Create(MetricPrefixes.Hecto, "h{0}");
		/// <inheritdoc cref="MetricPrefixes.Deca" />
		public static readonly NamedPrefix Deca = Create(MetricPrefixes.Deca, "da{0}");

		/// <inheritdoc cref="MetricPrefixes.Deci" />
		public static readonly NamedPrefix Deci = Create(MetricPrefixes.Deci, "d{0}");
		/// <inheritdoc cref="MetricPrefixes.Centi" />
		public static readonly NamedPrefix Centi = Create(MetricPrefixes.Centi, "c{0}");
		/// <inheritdoc cref="MetricPrefixes.Milli" />
		public static readonly NamedPrefix Milli = Create(MetricPrefixes.Milli, "m{0}");
		/// <inheritdoc cref="MetricPrefixes.Micro" />
		public static readonly NamedPrefix Micro = Create(MetricPrefixes.Micro, "μ{0}");
		/// <inheritdoc cref="MetricPrefixes.Nano" />
		public static readonly NamedPrefix Nano = Create(MetricPrefixes.Nano, "n{0}");

		[MethodImpl(MethodImplOptions.NoInlining)]
		static NamedMetricPrefixes() { }
	}
}
