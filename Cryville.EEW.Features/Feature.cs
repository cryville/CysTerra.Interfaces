using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Tag = System.Collections.Generic.KeyValuePair<Cryville.EEW.TagTypeKey, object?>;

namespace Cryville.EEW.Features {
	/// <summary>
	/// Represents a feature.
	/// </summary>
	/// <param name="Representation">The representation of the feature.</param>
	[SuppressMessage("CodeQuality", "IDE0079", Justification = "False report")]
	[SuppressMessage("Naming", "CA1710")]
	[DebuggerDisplay("{Representation}, Count = {Count}")]
	[DebuggerTypeProxy(typeof(FeatureDebugView))]
	public record Feature(object? Representation = null) : IDictionary<TagTypeKey, object?> {
		readonly Dictionary<TagTypeKey, object?> _tags = [];

		/// <summary>
		/// Returns the value of the copula tag of the feature as a type key, if present.
		/// </summary>
		/// <returns>The value of the copula tag of the feature as a type key, or <see langword="null" /> if the copula tag is not present.</returns>
		/// <exception cref="InvalidCastException">The value of the copula tag of the feature cannot be interpreted as a type key.</exception>
		public TagTypeKey? GetFeatureType() {
			if (!_tags.TryGetValue(SpecialTagTypeKeys.Is, out var type))
				return null;
			return type switch {
				TagTypeKey key => key,
				string str => str,
				_ => throw new InvalidCastException("Invalid value for the copula tag of a feature."),
			};
		}

		/// <inheritdoc/>
		[SuppressMessage("CodeQuality", "IDE0079", Justification = "False report")]
		[SuppressMessage("Design", "CA1043", Justification = "Interface implementation")]
		public object? this[TagTypeKey key] {
			get => _tags[key];
			set {
				_tags[key] = value;
			}
		}
		/// <inheritdoc/>
		public ICollection<TagTypeKey> Keys => _tags.Keys;
		/// <inheritdoc/>
		public ICollection<object?> Values => _tags.Values;
		/// <inheritdoc/>
		public int Count => _tags.Count;
		/// <inheritdoc/>
		public bool IsReadOnly => false;

		/// <inheritdoc/>
		public void Add(TagTypeKey key, object? value) {
			_tags.Add(key, value);
		}
		/// <inheritdoc/>
		public void Add(Tag item) => Add(item.Key, item.Value);
		/// <inheritdoc/>
		public void Clear() {
			_tags.Clear();
		}
		/// <inheritdoc/>
		public bool Contains(Tag item) => _tags.TryGetValue(item.Key, out var value) && value == item.Value;
		/// <inheritdoc/>
		public bool ContainsKey(TagTypeKey key) => _tags.ContainsKey(key);
		/// <inheritdoc/>
		public void CopyTo(Tag[] array, int arrayIndex) => ((ICollection<Tag>)_tags).CopyTo(array, arrayIndex);
		/// <summary>
		/// Returns an enumerator that iterates through the tags.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the tags.</returns>
		public Dictionary<TagTypeKey, object?>.Enumerator GetEnumerator() => _tags.GetEnumerator();
		IEnumerator<Tag> IEnumerable<Tag>.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		/// <inheritdoc/>
		public bool Remove(TagTypeKey key) {
			return _tags.Remove(key);
		}
		/// <inheritdoc/>
		public bool Remove(Tag item) => _tags.TryGetValue(item.Key, out var value) && value == item.Value && Remove(item.Key);
		/// <inheritdoc/>
		public bool TryGetValue(TagTypeKey key, out object? value) => _tags.TryGetValue(key, out value);
	}

	sealed class FeatureDebugView(Feature feature) {
		public object? Representation => feature.Representation;
		public Tag[] Tags {
			get {
				Tag[] array = new Tag[feature.Count];
				feature.CopyTo(array, 0);
				return array;
			}
		}
	}
}
