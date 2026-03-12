using Tag = System.Collections.Generic.KeyValuePair<Cryville.EEW.TagTypeKey, object?>;

namespace Cryville.EEW {
	/// <summary>
	/// Represents a generator that generates a tagged property from a tag.
	/// </summary>
	public interface ITaggedPropertyGenerator : IGenerator<Tag, TaggedProperty?>, ITagTypedGenerator { }
}
