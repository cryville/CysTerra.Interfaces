using Cryville.Common.Compat;
using System.Collections;
using System.Collections.Generic;

namespace Cryville.EEW {
	/// <summary>
	/// Provides a set of <see langword="static" /> methods related to values.
	/// </summary>
	public static class ValueHelpers {
		/// <summary>
		/// Gets the object itself if it is of the specified type, or the items of the specified type if the object is an enumerable.
		/// </summary>
		/// <typeparam name="T">The type of the items.</typeparam>
		/// <param name="obj">The object.</param>
		/// <returns>An enumerable containing the instances of the specified type within the object.</returns>
		public static IEnumerable<T> GetValues<T>(object? obj) {
			ThrowHelper.ThrowIfNull(obj);
			if (obj is T value) {
				yield return value;
			}
			else if (obj is IEnumerable values) {
				foreach (var v in values) {
					if (v is T v2) {
						yield return v2;
					}
				}
			}
		}
	}
}
