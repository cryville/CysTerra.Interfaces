using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace Cryville.EEW.Core {
	public static class Localization {
		public static void InitResources(ILocalizedResourceManager manager) => LocalizedResources.DefaultManager = manager;
		public static ResourceManagerScope EnterScope(IScopedLocalizedResourceManager manager) => LocalizedResources.EnterScope(manager);
		public static ResourceManagerScope EnterAssemblyScope(Assembly asm) => EnterScope(new AssemblyScopedLocalizedResourceManager(
			LocalizedResources.DefaultManager ?? throw new InvalidOperationException("Resource not initialized."),
			LocalizedResources.GetAssemblyName(asm)
		));
		struct AssemblyScopedLocalizedResourceManager(ILocalizedResourceManager parent, string assemblyName) : IScopedLocalizedResourceManager {
			CultureInfo _lastReturnedCulture = CultureInfo.InvariantCulture;
			public CultureInfo ReadLastReturnedCulture() => Interlocked.Exchange(ref _lastReturnedCulture, CultureInfo.InvariantCulture);
			public IMessageStringSet GetRootStringSet(string assembly, string type, [NotNull] ref CultureInfo? culture) {
				var result = parent.GetRootStringSet(assemblyName, type, ref culture);
				_lastReturnedCulture = culture;
				return result;
			}
			public readonly ILocalizableMessageStringSet GetLocalizableRootStringSet(string assembly, string type) => parent.GetLocalizableRootStringSet(assemblyName, type);
		}

		public static CultureInfo CurrentCulture {
			get => SharedCultures.CurrentCulture;
			set => SharedCultures.CurrentCulture = value;
		}
		public static CultureInfo CurrentUICulture {
			get => SharedCultures.CurrentUICulture;
			set => SharedCultures.CurrentUICulture = value;
		}
	}
}
