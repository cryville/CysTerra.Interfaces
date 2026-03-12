using System;
using System.ComponentModel;
using System.Globalization;

namespace Cryville.EEW {
	/// <summary>
	/// Represents a scope that overrides the resource manager temporarily.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly ref struct ResourceManagerScope : IDisposable {
		readonly IScopedLocalizedResourceManager _manager;
		internal ResourceManagerScope(IScopedLocalizedResourceManager manager) {
			if (LocalizedResources._overrideManager != null)
				throw new InvalidOperationException("Cannot re-enter resource manager scope.");
			LocalizedResources._overrideManager = _manager = manager;
		}
		/// <inheritdoc />
		public void Dispose() {
			if (_manager == null)
				return;
			LocalizedResources._overrideManager = null;
		}

		/// <summary>
		/// Gets and clears the culture last returned.
		/// </summary>
		public CultureInfo ReadLastReturnedCulture() => _manager.ReadLastReturnedCulture();
	}
}
