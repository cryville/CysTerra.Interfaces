using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Cryville.EEW {
	static class LocalizedResources {
		public static ILocalizedResourceManager? Manager => _overrideManager ?? DefaultManager;
		public static ILocalizedResourceManager? DefaultManager { get; set; }
		[ThreadStatic]
		internal static ILocalizedResourceManager? _overrideManager;
		public static ResourceManagerScope EnterScope(IScopedLocalizedResourceManager resourceManager) => new(resourceManager);

		static readonly ConcurrentDictionary<Assembly, string> _asmNameCache = [];
		public static string GetAssemblyName(Assembly assembly) {
			if (!_asmNameCache.TryGetValue(assembly, out string? name)) {
				_asmNameCache.TryAdd(assembly, name = assembly.GetName().Name ?? throw new InvalidOperationException("Invalid resource context."));
			}
			return name;
		}
	}
}
