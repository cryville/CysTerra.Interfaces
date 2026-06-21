using System;

namespace Cryville.Packages {
	static class Shared {
		public static readonly string UserAgent = typeof(Shared).Assembly.GetName().Version is Version ver ? $"CryvillePackages/{ver.ToString(3)}" : "CryvillePackages";
	}
}
