using Cryville.Common.Compat;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Cryville.EEW {
	/// <summary>
	/// Provides a set of <see langword="static" /> methods related to <see cref="IMessageStringSet" />.
	/// </summary>
	public static class MessageStringSetExtensions {
		/// <summary>
		/// Gets a string in the string set.
		/// </summary>
		/// <param name="set">The string set.</param>
		/// <param name="name">The name of the string.</param>
		/// <returns>The string of the specified name in the resource.</returns>
		/// <exception cref="KeyNotFoundException">The string of the specified name is not found.</exception>
		public static string GetStringRequired(this IMessageStringSet set, string name) {
			ThrowHelper.ThrowIfNull(set);
			return set.GetString(name) ?? throw new KeyNotFoundException($"The string of name {name} is not found in the string set.");
		}
		/// <summary>
		/// Gets a string in the string set, or a default string in the string set if not found.
		/// </summary>
		/// <param name="set">The string set.</param>
		/// <param name="name">The name of the string.</param>
		/// <param name="defaultName">The name of the default string.</param>
		/// <returns>The string of the specified name in the resource, or the default string of the specified default name in the string set if not found.</returns>
		/// <exception cref="KeyNotFoundException">The default string of the specified default name is not found.</exception>
		public static string GetStringOrDefault(this IMessageStringSet set, string name, string defaultName = "") {
			ThrowHelper.ThrowIfNull(set);
			return set.GetString(name) ?? set.GetStringRequired(defaultName);
		}
		/// <summary>
		/// Gets a string set in the string set.
		/// </summary>
		/// <param name="set">The string set.</param>
		/// <param name="name">The name of the string set.</param>
		/// <returns>The string set of the specified name in the string set.</returns>
		/// <exception cref="KeyNotFoundException">The string set of the specified name is not found.</exception>
		public static IMessageStringSet GetStringSetRequired(this IMessageStringSet set, string name) {
			ThrowHelper.ThrowIfNull(set);
			return set.GetStringSet(name) ?? throw new KeyNotFoundException($"The string set of name {name} is not found in the string set.");
		}

		/// <summary>
		/// Gets a string in the string set.
		/// </summary>
		/// <param name="set">The string set.</param>
		/// <param name="name">The name of the string.</param>
		/// <returns>The string of the specified name in the resource.</returns>
		public static ILocalizable<string> GetStringRequired(this ILocalizableMessageStringSet set, string name) {
			ThrowHelper.ThrowIfNull(set);
			return new RequiredLocalizableString(set.GetString(name), name);
		}
		/// <summary>
		/// Gets a string in the string set, or a default string in the string set if not found.
		/// </summary>
		/// <param name="set">The string set.</param>
		/// <param name="name">The name of the string.</param>
		/// <param name="defaultName">The name of the default string.</param>
		/// <returns>The string of the specified name in the resource, or the default string of the specified default name in the string set if not found.</returns>
		public static ILocalizable<string> GetStringOrDefault(this ILocalizableMessageStringSet set, string name, string defaultName = "") {
			ThrowHelper.ThrowIfNull(set);
			return new FallbackLocalizableString(set.GetString(name), set.GetString(defaultName), defaultName);
		}
		sealed record RequiredLocalizableString(ILocalizable<string?> Value, string Name) : ILocalizable<string> {
			public string GetLocalizedValue([NotNull] ref CultureInfo? culture) => Value.GetLocalizedValue(ref culture) ?? throw new KeyNotFoundException($"The string of name {Name} is not found in the string set.");
		}
		sealed record FallbackLocalizableString(ILocalizable<string?> Value, ILocalizable<string?> FallbackValue, string FallbackName) : ILocalizable<string> {
			public string GetLocalizedValue([NotNull] ref CultureInfo? culture) {
				var primaryCulture = culture;
				var primaryString = Value.GetLocalizedValue(ref primaryCulture);
				string primaryCultureName = primaryCulture.Name;

				var fallbackCulture = culture;
				var fallbackString = FallbackValue.GetLocalizedValue(ref fallbackCulture);
				string fallbackCultureName = fallbackCulture.Name;

				culture ??= CultureInfo.InvariantCulture;
				for (; ; ) {
					string cultureName = culture.Name;
					if (cultureName == primaryCultureName && primaryString != null) {
						return primaryString;
					}
					if (cultureName == fallbackCultureName && fallbackString != null) {
						return fallbackString;
					}
					if (culture.Equals(CultureInfo.InvariantCulture)) {
						throw new KeyNotFoundException($"The string of name {FallbackName} is not found in the string set.");
					}
					culture = culture.Parent;
				}
			}
		}
	}
}
