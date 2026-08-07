using System.Diagnostics.CodeAnalysis;

namespace Cryville.Packages {
	public interface ILocalPackageRepository {
		string PlatformName { get; }
		bool TryGetLocalPackageVersion(string package, out string? version);
		string GetTempFilePath(string fileName);
	}
	public interface ILocalPackageRepository2 : ILocalPackageRepository {
		bool ILocalPackageRepository.TryGetLocalPackageVersion(string package, out string? version) {
			if (TryGetLocalPackageInfo(package, out var info)) {
				version = info.Version;
				return true;
			}
			version = null;
			return false;
		}
		bool TryGetLocalPackageInfo(string package, [NotNullWhen(true)] out LocalPackageInfo? info);
	}
	public sealed record LocalPackageInfo(string? Version, bool IsInternalPackage = false);
}
