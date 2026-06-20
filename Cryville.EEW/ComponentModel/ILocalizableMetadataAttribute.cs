namespace Cryville.EEW.ComponentModel {
	/// <summary>
	/// Represents an metadata attribute that is localizable with <see cref="LocalizedResource" />.
	/// </summary>
	public interface ILocalizableMetadataAttribute : ILocalizable<string> {
		/// <summary>
		/// The name of the localized resource.
		/// </summary>
		string Type { get; }
		/// <summary>
		/// The string set names where the localizable string is.
		/// </summary>
		string[] Path { get; }
		/// <summary>
		/// The name of the localizable string.
		/// </summary>
		string Name { get; }
	}
}