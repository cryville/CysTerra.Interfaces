using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Cryville.EEW {
	/// <summary>
	/// Represent an object holding a localized value.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	public interface ILocalizable<out T> {
		/// <summary>
		/// Gets the value of a culture.
		/// </summary>
		/// <param name="culture">The preferred culture of the value. When the method returns, set to the actual culture of the value.</param>
		/// <returns>The localized value.</returns>
		T GetLocalizedValue([NotNull] ref CultureInfo? culture);
	}
}
