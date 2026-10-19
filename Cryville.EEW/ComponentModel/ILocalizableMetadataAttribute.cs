using System.Diagnostics.CodeAnalysis;

namespace Cryville.EEW.ComponentModel {
	/// <summary>
	/// Represents an metadata attribute that is localizable with <see cref="LocalizedResource" />.
	/// </summary>
	[SuppressMessage("Naming", "CA1711", Justification = "Attribute type")]
	public interface ILocalizableMetadataAttribute : ILocalizable<string> {
		/// <summary>
		/// The name of the localized resource.
		/// </summary>
		string Type { get; }
		/// <summary>
		/// The string set names where the localizable string is.
		/// </summary>
		[SuppressMessage("Performance", "CA1819", Justification = "Attribute type")]
		string[] Path { get; }
		/// <summary>
		/// The name of the localizable string.
		/// </summary>
		string Name { get; }
	}
}