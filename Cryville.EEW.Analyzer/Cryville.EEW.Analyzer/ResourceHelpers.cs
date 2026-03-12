using Microsoft.CodeAnalysis;

namespace Cryville.EEW.Analyzer {
	static class ResourceHelpers {
		public static LocalizableResourceString CreateLocalizableResourceString(string name) => new(name, Resources.ResourceManager, typeof(Resources));
	}
}
