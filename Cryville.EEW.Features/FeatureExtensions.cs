using Cryville.Common.Compat;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Cryville.EEW.Features {
	/// <summary>
	/// Provides a set of extension methods related to the <see cref="Feature" /> class.
	/// </summary>
	public static class FeatureExtensions {
		/// <summary>
		/// Gets the feature itself if its representation is of the specified type, or the value of the location tag as a feature if it is of the specified type, or <see langword="null" />.
		/// </summary>
		/// <typeparam name="T">The type of the representation.</typeparam>
		/// <param name="f">The feature.</param>
		/// <param name="representation">When the method returns, set to the representation of the feature returned, or <see langword="null" /> if <see langword="null" /> is returned.</param>
		/// <returns>The feature itself if its representation is of the specified type, or the value of the location tag as a feature if it is of the specified type, or <see langword="null" />.</returns>
		[return: NotNullIfNotNull(nameof(representation))]
		public static Feature? GetFeatureOfRepresentation<T>(this Feature f, out T? representation) {
			ThrowHelper.ThrowIfNull(f);
			if (f.Representation is T r) {
				representation = r;
				return f;
			}
			else if (f.TryGetValue<Feature>(SpecialTagTypeKeys.At, out var lf) && lf.Representation is T sr) {
				representation = sr;
				return lf;
			}
			representation = default;
			return null;
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="f">The feature.</param>
		/// <param name="key">The key whose value to get.</param>
		/// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
		/// <returns><see langword="true" /> if the feature contains an tag with the specified key; otherwise, <see langword="false" />.</returns>
		public static bool TryGetValue<T>(this Feature f, TagTypeKey key, [NotNullWhen(true)] out T? value) {
			ThrowHelper.ThrowIfNull(f);
			if (!f.TryGetValue(key, out var result)) {
				value = default;
				return false;
			}
			if (result is not T resultValue) {
				value = default;
				return false;
			}
			value = resultValue;
			return true;
		}

		/// <summary>
		/// Gets the value of the specified key if it is of the specified type, or the items of the specified type if the value is an enumerable.
		/// </summary>
		/// <typeparam name="T">The type of the items.</typeparam>
		/// <param name="f">The feature.</param>
		/// <param name="key">The key.</param>
		/// <returns>An enumerable containing the instances of the specified type within the value of the specified key.</returns>
		public static IEnumerable<T> GetValues<T>(this Feature f, TagTypeKey key) {
			ThrowHelper.ThrowIfNull(f);
			if (!f.TryGetValue(key, out var result))
				return [];
			return ValueHelpers.GetValues<T>(result);
		}
	}
}
