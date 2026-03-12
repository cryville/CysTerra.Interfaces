using Cryville.Common.Compat;
using System.Globalization;

namespace Cryville.EEW {
	/// <summary>
	/// Provides a set of <see langword="static" /> methods related to the <see cref="ILocalizable{T}" /> interface.
	/// </summary>
	public static class LocalizableExtensions {
		/// <summary>
		/// Gets the value of a culture.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="localized">The localized value holder.</param>
		/// <param name="culture">The preferred culture of the value.</param>
		/// <returns>An instance of the <see cref="Localized{T}" /> struct representing the already localized value with its culture.</returns>
		public static Localized<T> GetLocalizedValue<T>(this ILocalizable<T> localized, CultureInfo culture) {
			ThrowHelper.ThrowIfNull(localized);
			var value = localized.GetLocalizedValue(ref culture);
			return new(value, culture);
		}
	}
}
