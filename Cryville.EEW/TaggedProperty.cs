using System.Globalization;
using Tag = System.Collections.Generic.KeyValuePair<Cryville.EEW.TagTypeKey, object?>;

namespace Cryville.EEW {
	/// <summary>
	/// Represents a property derived from a tag.
	/// </summary>
	/// <param name="Tag">The tag.</param>
	/// <param name="Key">The displayed name.</param>
	/// <param name="Value">The displayed value.</param>
	/// <param name="Culture">The culture.</param>
	/// <param name="IsUncomparable">Whether the property is uncomparable and cannot derive a severity and a color.</param>
	public record TaggedProperty(Tag Tag, string? Key, string Value, CultureInfo Culture, bool IsUncomparable = false) {
		/// <summary>
		/// The type key of the tag.
		/// </summary>
		public TagTypeKey Type => Tag.Key;
		/// <summary>
		/// The value of the tag.
		/// </summary>
		public object? RawValue => Tag.Value;
		/// <summary>
		/// The additional condition.
		/// </summary>
		public string? Condition { get; init; }
		/// <summary>
		/// The culture.
		/// </summary>
		public CultureInfo Culture { get; internal set; } = Culture;

		/// <summary>
		/// Gets a tag as a hint to color mapping for the property.
		/// </summary>
		/// <returns>A tag as a hint to color mapping for the property.</returns>
		public virtual Tag? GetTagForColorHint() => IsUncomparable ? null : Tag;
	}
}
