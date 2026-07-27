using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
			if (Count == 0)
				throw new InvalidOperationException("Empty localizable collection.");
			if (LocalizedResources.Manager is ILocalizedResourceManager2 manager2)
				return GetLocalizedValue(manager2, ref culture);
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

		T GetLocalizedValue(ILocalizedResourceManager2 manager2, [NotNull] ref CultureInfo? culture) {
			Debug.Assert(Count > 0);
			var desiredCulture = culture ?? CultureInfo.InvariantCulture;
			int minDistance = int.MaxValue;
			T? resultValue = default;
			CultureInfo? resultCulture = null;
			foreach (var item in this) {
				var itemCulture = desiredCulture;
				var value = item.GetLocalizedValue(ref itemCulture);

				int distance = manager2.GetCultureDistance(desiredCulture, itemCulture);
				if (resultCulture == null || distance < minDistance) {
					minDistance = distance;
					resultValue = value;
					resultCulture = itemCulture;
				}
			}
#pragma warning disable CS8777
			culture = resultCulture;
			return resultValue!;
#pragma warning restore CS8777
		}
	}
}
