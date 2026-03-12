namespace Cryville.EEW {
	/// <summary>
	/// Represents a generator that is matched against a tag type key.
	/// </summary>
	public interface ITagTypedGenerator {
		/// <summary>
		/// The type key.
		/// </summary>
		TagTypeKey TypeKey { get; }
	}
}
