using Cryville.Common.Compat;
using Cryville.Measure;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Cryville.EEW.Measure {
	/// <summary>
	/// A unit with a symbol and names.
	/// </summary>
	/// <param name="Unit">The unit.</param>
	/// <param name="Symbol">The culture-independent symbol.</param>
	/// <param name="ShortName">The short name.</param>
	/// <param name="FullName">The full name.</param>
	public readonly record struct NamedUnit(Unit Unit, string? Symbol, ILocalizable<string> ShortName, ILocalizable<string> FullName) {
		/// <summary>
		/// Creates an instance of the <see cref="NamedUnit" /> struct.
		/// </summary>
		/// <param name="unit">The unit.</param>
		/// <param name="symbol">The culture-independent symbol.</param>
		/// <param name="name">The name.</param>
		public NamedUnit(Unit unit, string? symbol, ILocalizable<string> name) : this(unit, symbol, name, name) { }

		/// <summary>
		/// Creates a unit based on the current unit modified by the specified prefix.
		/// </summary>
		/// <param name="prefix">The prefix.</param>
		/// <returns>The new unit.</returns>
		public NamedUnit WithPrefix(NamedPrefix prefix) => new(
			Unit.WithPrefix(prefix.Scale),
			Symbol != null ? string.Format(CultureInfo.InvariantCulture, prefix.SymbolAffix, Symbol) : null,
			new AffixedLocalizableString(ShortName, prefix.ShortNameAffix),
			new AffixedLocalizableString(FullName, prefix.FullNameAffix)
		);

		/// <summary>
		/// Formats a quantity with the current named unit.
		/// </summary>
		/// <param name="valueStr">The string representation of the value.</param>
		/// <param name="culture">The preferred culture. When the method returns, set to the actual culture used in formatting.</param>
		/// <param name="unitStyle">The unit style.</param>
		/// <returns>A string representation of the given quantity in the current named unit affixed with the name.</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public string FormatQuantity(string valueStr, [NotNull] ref CultureInfo? culture, UnitStyle unitStyle) {
			var unitNameCulture = culture;
			string format = new LocalizedResource("", ref culture).RootMessageStringSet.GetStringRequired("QuantityFormat");
			return string.Format(culture, format, valueStr, GetUnitName(unitStyle, ref unitNameCulture));
		}

		/// <summary>
		/// Gets the unit name of the specified style.
		/// </summary>
		/// <param name="style">The unit style.</param>
		/// <param name="culture">The preferred culture of the name. When the method returns, set to the actual culture of the name.</param>
		/// <returns>The unit name of the specified style.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="style" /> is unknown.</exception>
		public string? GetUnitName(UnitStyle style, [NotNull] ref CultureInfo? culture) {
			culture ??= CultureInfo.InvariantCulture;
			return style switch {
				UnitStyle.Symbol => Symbol,
				UnitStyle.ShortName => ShortName.GetLocalizedValue(ref culture),
				UnitStyle.FullName => FullName.GetLocalizedValue(ref culture),
				_ => throw new ArgumentOutOfRangeException(nameof(style)),
			};
		}

		/// <summary>
		/// Returns the multiplication of two units.
		/// </summary>
		/// <param name="left">The first unit.</param>
		/// <param name="right">The second unit.</param>
		/// <returns>The multiplication of the two units.</returns>
		public static NamedUnit Multiply(NamedUnit left, NamedUnit right) {
			ThrowHelper.ThrowIfNull(left);
			ThrowHelper.ThrowIfNull(right);
			var nameTemplate = new LocalizableResource("").RootMessageStringSet.GetStringRequired("MultiplyFormat");
			return new(
				left.Unit * right.Unit,
				string.Format(CultureInfo.InvariantCulture, "{0}\xb7{1}", left.Symbol, right.Symbol),
				new CompositeUnitLocalizableString(nameTemplate, left.ShortName, right.ShortName),
				new CompositeUnitLocalizableString(nameTemplate, left.FullName, right.FullName)
			);
		}
		/// <inheritdoc cref="Multiply(NamedUnit, NamedUnit)" />
		public static NamedUnit operator *(NamedUnit left, NamedUnit right) => Multiply(left, right);
		/// <summary>
		/// Returns the division of a unit divided by another.
		/// </summary>
		/// <param name="left">The divided unit.</param>
		/// <param name="right">The dividing unit.</param>
		/// <returns>The division of <paramref name="left" /> divided by <paramref name="right" />.</returns>
		public static NamedUnit Divide(NamedUnit left, NamedUnit right) {
			ThrowHelper.ThrowIfNull(left);
			ThrowHelper.ThrowIfNull(right);
			var nameTemplate = new LocalizableResource("").RootMessageStringSet.GetStringRequired("DivideFormat");
			return new(
				left.Unit / right.Unit,
				string.Format(CultureInfo.InvariantCulture, "{0}/{1}", left.Symbol, right.Symbol),
				new CompositeUnitLocalizableString(nameTemplate, left.ShortName, right.ShortName),
				new CompositeUnitLocalizableString(nameTemplate, left.FullName, right.FullName)
			);
		}
		/// <inheritdoc cref="Divide(NamedUnit, NamedUnit)" />
		public static NamedUnit operator /(NamedUnit left, NamedUnit right) => Divide(left, right);
	}
	sealed record AffixedLocalizableString(ILocalizable<string> Base, ILocalizable<string> Affix) : ILocalizable<string> {
		public string GetLocalizedValue([NotNull] ref CultureInfo? culture) {
			string baseStr = Base.GetLocalizedValue(ref culture);
			var affixCulture = culture;
			return string.Format(culture, Affix.GetLocalizedValue(ref affixCulture), baseStr);
		}
	}
	sealed record CompositeUnitLocalizableString(ILocalizable<string> Template, ILocalizable<string> Left, ILocalizable<string> Right) : ILocalizable<string> {
		public string GetLocalizedValue([NotNull] ref CultureInfo? culture) {
			string leftName = Left.GetLocalizedValue(ref culture);
			CultureInfo templateCulture = culture, rightCulture = culture;
			return string.Format(culture, Template.GetLocalizedValue(ref templateCulture), leftName, Right.GetLocalizedValue(ref rightCulture));
		}
	}
}
