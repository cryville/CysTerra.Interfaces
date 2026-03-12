using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Cryville.EEW.ComponentModel {
	/// <summary>
	/// Represents a named component.
	/// </summary>
	public interface INamedComponent {
		/// <summary>
		/// Gets a human-readable name of the component.
		/// </summary>
		/// <param name="culture">The preferred culture of the name. When the method returns, set to the actual culture of the name.</param>
		/// <remarks>
		/// It is recommended to get the name from a <see cref="LocalizedResource" />.
		/// </remarks>
		string? GetName([NotNull] ref CultureInfo? culture);
	}
}
