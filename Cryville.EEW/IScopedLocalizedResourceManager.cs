using System.ComponentModel;
using System.Globalization;

namespace Cryville.EEW {
	/// <summary>
	/// Represents a scoped <see cref="ILocalizedResourceManager" />.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IScopedLocalizedResourceManager : ILocalizedResourceManager {
		/// <summary>
		/// Gets and clears the culture last returned by <see cref="ILocalizedResourceManager.GetRootStringSet(string, string, ref CultureInfo?)" />.
		/// </summary>
		CultureInfo ReadLastReturnedCulture();
	}
}
