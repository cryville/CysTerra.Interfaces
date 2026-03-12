using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Cryville.EEW {
	/// <summary>
	/// Represents a manager where localized resources are retrieved.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface ILocalizedResourceManager {
		/// <summary>
		/// Gets a root string set.
		/// </summary>
		/// <param name="assembly">The name of the assembly.</param>
		/// <param name="type">The name of the resource.</param>
		/// <param name="culture">The preferred culture of the resource. When the method returns, set to the actual culture of the resource.</param>
		/// <returns>The root string set of the specified namespace.</returns>
		IMessageStringSet GetRootStringSet(string assembly, string type, [NotNull] ref CultureInfo? culture);

		/// <summary>
		/// Gets a localizable root string set.
		/// </summary>
		/// <param name="assembly">The name of the assembly.</param>
		/// <param name="type">The name of the resource.</param>
		/// <returns>The localizable root string set of the specified namespace.</returns>
		ILocalizableMessageStringSet GetLocalizableRootStringSet(string assembly, string type);
	}
}
