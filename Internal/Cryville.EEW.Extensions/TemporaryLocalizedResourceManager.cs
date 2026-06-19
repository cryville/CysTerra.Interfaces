using Cryville.EEW.Core;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Cryville.EEW.Extensions {
	public sealed class TemporaryLocalizedResourceManager(string basePath) : JSONFileLocalizedResourceManager, IScopedLocalizedResourceManager {
		CultureInfo _lastReturnedCulture = CultureInfo.InvariantCulture;
		public CultureInfo ReadLastReturnedCulture() => Interlocked.Exchange(ref _lastReturnedCulture, CultureInfo.InvariantCulture);

		public override IMessageStringSet GetRootStringSet(string assembly, string type, [NotNull] ref CultureInfo? culture) {
			var result = base.GetRootStringSet(assembly, type, ref culture);
			_lastReturnedCulture = culture;
			return result;
		}

		protected override Stream? Open(string ns, string path) => Open(path);
		protected override Stream? Open(string path) {
			string filePath = Path.Combine(basePath, "Messages", path);
			if (!File.Exists(filePath)) return null;
			return new FileStream(filePath, FileMode.Open, FileAccess.Read);
		}
	}
}
