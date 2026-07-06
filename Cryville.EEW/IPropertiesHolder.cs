using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Cryville.EEW {
	/// <summary>
	/// Represents an object that holds a set of properties.
	/// </summary>
	public interface IPropertiesHolder {
		/// <summary>
		/// Gets the properties of the object.
		/// </summary>
		/// <returns>The properties of the object.</returns>
#if NET5_0_OR_GREATER
		[System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered.")]
#endif
		IEnumerable<PropertyDescriptor> GetProperties() => TypeDescriptor.GetProperties(this).OfType<PropertyDescriptor>().Where(p => p.IsBrowsable && !p.IsReadOnly);
	}
}
