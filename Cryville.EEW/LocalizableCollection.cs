using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Cryville.EEW {
	/// <summary>
	/// Collection of <see cref="ILocalizable{T}" />.
	/// </summary>
	/// <typeparam name="T">The type of the values of the localizables.</typeparam>
	public class LocalizableCollection<T> : Collection<ILocalizable<T>>, ILocalizable<T> {
		/// <inheritdoc />
		public T GetLocalizedValue([NotNull] ref CultureInfo? culture) {
			culture ??= CultureInfo.InvariantCulture;
			foreach (var item in this) {
				var itemCulture = culture;
				var value = item.GetLocalizedValue(ref itemCulture);
				if (culture.Name == itemCulture.Name) {
					culture = itemCulture;
					return value;
				}
			}
			return this[0].GetLocalizedValue(ref culture);
		}
	}
}
