using Cryville.Common.Compat;
using System;
using System.Collections.Concurrent;

namespace Cryville.EEW {
	/// <summary>
	/// Represents a type key of a tag.
	/// </summary>
	public sealed record TagTypeKey {
		/// <summary>
		/// The name of the type.
		/// </summary>
		public string Name { get; }
		/// <summary>
		/// The parent type.
		/// </summary>
		public TagTypeKey? Parent { get; }
		/// <summary>
		/// The root type.
		/// </summary>
		public TagTypeKey Root => Parent ?? this;

		TagTypeKey(string name, TagTypeKey? parent = null) {
			ThrowHelper.ThrowIfNull(name);
			if (name.Contains(':', StringComparison.Ordinal) && name.Length > 1)
				throw new ArgumentException("Invalid type name.");
			Name = name;
			Parent = parent;
		}

		/// <summary>
		/// Creates a sub-type from this type.
		/// </summary>
		/// <param name="name">The name of the sub-type.</param>
		/// <returns>The created sub-type.</returns>
		public TagTypeKey OfSubtype(string name) => Of(name, this);

		/// <inheritdoc/>
		public override string ToString() => Parent is null ? Name : $"{Parent}:{Name}";

		static readonly ConcurrentDictionary<(string, TagTypeKey?), TagTypeKey> _instances = [];

		/// <summary>
		/// Gets an instance of the <see cref="TagTypeKey" /> class.
		/// </summary>
		/// <param name="name">The name of the type.</param>
		/// <param name="parent">The parent type.</param>
		/// <exception cref="ArgumentException"><paramref name="name" /> contains the sub-type delimiter <c>:</c>.</exception>
		public static TagTypeKey Of(string name, TagTypeKey? parent = null) {
			var key = (name, parent);
			if (!_instances.TryGetValue(key, out var instance)) {
				_instances.TryAdd(key, instance = new(name, parent));
			}
			return instance;
		}

		/// <summary>
		/// Creates a tag type key from a string.
		/// </summary>
		/// <param name="value">The name of the type.</param>
		/// <returns>The created type.</returns>
		public static TagTypeKey FromString(string value) {
			ThrowHelper.ThrowIfNull(value);
			if (value == ":") return Of(":");
			int index = value.LastIndexOf(':');
			if (index == -1) return Of(value);
			return Of(value[(index + 1)..], value[..index]);
		}
		/// <summary>
		/// Converts a string to a tag type key.
		/// </summary>
		/// <param name="value">The value.</param>
		public static implicit operator TagTypeKey(string value) => FromString(value);
	}
}
