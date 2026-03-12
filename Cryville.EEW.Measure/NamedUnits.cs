using Cryville.Measure;
using System.Runtime.CompilerServices;

namespace Cryville.EEW.Measure {
	/// <summary>
	/// Provides a set of predefined named units.
	/// </summary>
	public static class NamedUnits {
		static readonly ILocalizableMessageStringSet _res = new LocalizableResource("").RootMessageStringSet;
		static NamedUnit Create(Unit unit, string symbol, [CallerMemberName] string nameKey = "") =>
			new(unit, symbol, _res.GetStringOrDefault(nameKey + "Short", nameKey), _res.GetStringRequired(nameKey));

		/// <inheritdoc cref="Units.Dimensionless" />
		public static readonly NamedUnit Dimensionless = Create(Units.Dimensionless, "");
		/// <inheritdoc cref="Units.Second" />
		public static readonly NamedUnit Second = Create(Units.Second, "s");
		/// <inheritdoc cref="Units.Metre" />
		public static readonly NamedUnit Metre = Create(Units.Metre, "m");
		/// <inheritdoc cref="Units.Gram" />
		public static readonly NamedUnit Gram = Create(Units.Gram, "g");
		/// <inheritdoc cref="Units.Kilogram" />
		public static readonly NamedUnit Kilogram = Gram.WithPrefix(NamedMetricPrefixes.Kilo);
		/// <inheritdoc cref="Units.Ampere" />
		public static readonly NamedUnit Ampere = Create(Units.Ampere, "A");
		/// <inheritdoc cref="Units.Kelvin" />
		public static readonly NamedUnit Kelvin = Create(Units.Kelvin, "K");
		/// <inheritdoc cref="Units.Mole" />
		public static readonly NamedUnit Mole = Create(Units.Mole, "mol");
		/// <inheritdoc cref="Units.Candela" />
		public static readonly NamedUnit Candela = Create(Units.Candela, "cd");

		[MethodImpl(MethodImplOptions.NoInlining)]
		static NamedUnits() { }
	}
}
