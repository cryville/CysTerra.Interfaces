namespace Cryville.Packages {
	public interface ILocalPackageRepository {
		string PlatformName { get; }
		bool TryGetLocalPackageVersion(string package, out string? version);
		string GetTempFilePath(string fileName);
	}
}
