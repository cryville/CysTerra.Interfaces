using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Cryville.EEW {
	/// <summary>
	/// Represent an object holding an already localized value.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="Value">The value.</param>
	/// <param name="Culture">The culture.</param>
	public record struct Localized<T>(T Value, CultureInfo Culture) : ILocalizable<T> {
		/// <inheritdoc />
		public readonly T GetLocalizedValue([NotNull] ref CultureInfo? culture) {
			culture = Culture;
			return Value;
		}
		/// <inheritdoc />
		public override readonly string? ToString() => Value?.ToString();
	}
}
