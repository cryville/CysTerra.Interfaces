using System;
using System.Reflection;

namespace Cryville.EEW {
	class WebUtils {
		public static string? ToUserAgent(Type type) => ToUserAgent(type.Assembly);
		public static string? ToUserAgent(Assembly assembly) => ToUserAgent(assembly.GetName());
		public static string? ToUserAgent(AssemblyName assemblyName) => assemblyName.Version is Version version ? $"{assemblyName.Name}/{version}" : assemblyName.Name;
	}
}