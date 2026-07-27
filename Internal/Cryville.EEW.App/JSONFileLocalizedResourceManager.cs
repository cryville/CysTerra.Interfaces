using Cryville.Common.Compat;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cryville.EEW.Core {
	public abstract partial class JSONFileLocalizedResourceManager : ILocalizedResourceManager {
		readonly ConcurrentDictionary<(string, string, CultureInfo), MessageRootStringSet> _loadedMessagesByInput = [];
		readonly ConcurrentDictionary<(string, string), MessageRootStringSet?> _loadedMessagesByPath = [];

		public virtual IMessageStringSet GetRootStringSet(string assembly, string type, [NotNull] ref CultureInfo? culture)
			=> GetRootStringSetInternal(assembly, type, ref culture);
		MessageRootStringSet GetRootStringSetInternal(string assembly, string type, [NotNull] ref CultureInfo? culture) {
			ThrowHelper.ThrowIfNull(type);

			culture ??= CultureInfo.InvariantCulture;
			var key = (assembly, type, culture);
			if (!_loadedMessagesByInput.TryGetValue(key, out var set)) {
				set = OpenRootStringSetWithCultureFallback(assembly, type, ref culture);
				_loadedMessagesByInput.TryAdd(key, set);
			}
			culture = SharedCultures.Get(set.Culture);
			return set;
		}
		protected virtual MessageRootStringSet OpenRootStringSetWithCultureFallback(string assembly, string type, ref CultureInfo culture) {
			for (; ; ) {
				bool isUltimateFallback = culture.Equals(CultureInfo.InvariantCulture);
				var set = GetOrOpenRootStringSetWithCultureExact(assembly, type, culture.Name, isUltimateFallback);
				if (set == null) {
					if (isUltimateFallback)
						throw new InvalidOperationException("Resource not found.");
					culture = culture.Parent;
					continue;
				}
				return set;
			}
		}
		protected MessageRootStringSet? GetOrOpenRootStringSetWithCultureExact(string assembly, string type, string cultureName, bool isUltimateFallback) {
			string path = Path.Combine(type, $"{(isUltimateFallback ? "und" : cultureName)}.json");
			var pathKey = (assembly, path);
			if (!_loadedMessagesByPath.TryGetValue(pathKey, out var set)) {
				using var stream = Open(assembly, path);
				set = stream == null ? null : JsonSerializer.Deserialize(stream, SerializerContext.Default.MessageRootStringSet);
				if (set?.Parent is string parent) {
					var parentCulture = SharedCultures.Get(parent);
					set._parentSet = GetRootStringSet(assembly, type, ref parentCulture);
				}
				_loadedMessagesByPath.TryAdd(pathKey, set);
			}
			return set;
		}

		protected virtual Stream? Open(string ns, string path) => Open(Path.Combine(ns, path));
		protected abstract Stream? Open(string path);

		readonly ConcurrentDictionary<(string, string), LocalizableMessageRootStringSet> _localizableByInput = [];
		public ILocalizableMessageStringSet GetLocalizableRootStringSet(string assembly, string type) {
			var key = (assembly, type);
			if (!_localizableByInput.TryGetValue(key, out var set)) {
				_localizableByInput.TryAdd(key, set = new LocalizableMessageRootStringSet(this, assembly, type));
			}
			return set;
		}

		protected record MessageStringSet(
			IReadOnlyDictionary<string, string> Strings,
			IReadOnlyDictionary<string, MessageStringSet> StringSets
		) : IMessageStringSet {
			internal IMessageStringSet? _parentSet;
			public string? GetString(string name) => Strings?.GetValueOrDefault(name) ?? _parentSet?.GetString(name);
			public IMessageStringSet? GetStringSet(string name) {
				var result = StringSets?.GetValueOrDefault(name);
				if (result == null)
					return _parentSet?.GetStringSet(name);
				if (_parentSet != null)
					result._parentSet ??= _parentSet.GetStringSet(name);
				return result;
			}
		}

		protected sealed record MessageRootStringSet(
			string Culture,
			string? Parent,
			IReadOnlyDictionary<string, string> Strings,
			IReadOnlyDictionary<string, MessageStringSet> StringSets
		) : MessageStringSet(Strings, StringSets) { }

		[JsonSerializable(typeof(MessageRootStringSet))]
		sealed partial class SerializerContext : JsonSerializerContext { }

		abstract record LocalizableMessageStringSetBase : ILocalizableMessageStringSet {
			public ILocalizable<string?> GetString(string name) => new LocalizableString(this, name);
			public ILocalizableMessageStringSet GetStringSet(string name) => new LocalizableMessageStringSet(this, name);

			public abstract string? GetString(string name, [NotNull] ref CultureInfo? culture);
			public abstract IMessageStringSet? GetStringSet(string name, [NotNull] ref CultureInfo? culture);
		}
		sealed record LocalizableMessageRootStringSet(JSONFileLocalizedResourceManager Manager, string Assembly, string Type) : LocalizableMessageStringSetBase {
			public override string? GetString(string name, [NotNull] ref CultureInfo? culture) => Manager.GetRootStringSet(Assembly, Type, ref culture).GetString(name);
			public override IMessageStringSet? GetStringSet(string name, [NotNull] ref CultureInfo? culture) => Manager.GetRootStringSet(Assembly, Type, ref culture).GetStringSet(name);
		}
		sealed record LocalizableMessageStringSet(LocalizableMessageStringSetBase Parent, string Name) : LocalizableMessageStringSetBase {
			public override string? GetString(string name, [NotNull] ref CultureInfo? culture) => Parent.GetStringSet(Name, ref culture)?.GetString(name);
			public override IMessageStringSet? GetStringSet(string name, [NotNull] ref CultureInfo? culture) => Parent.GetStringSet(Name, ref culture)?.GetStringSet(name);
		}
		sealed record LocalizableString(LocalizableMessageStringSetBase Parent, string Name) : ILocalizable<string?> {
			public string? GetLocalizedValue([NotNull] ref CultureInfo? culture) => Parent.GetString(Name, ref culture);
		}
	}
}
