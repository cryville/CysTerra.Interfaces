using Cryville.Common.Compat;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cryville.Packages {
	public class PackageInfoCollection : KeyedCollection<string, PackageInfo> {
		protected override string GetKeyForItem(PackageInfo item) {
			ThrowHelper.ThrowIfNull(item);
			return item.Id;
		}
	}
	public record PackageInfo(
		string Id,
		PlatformPackageInfoCollection Platforms
	);

	public class PlatformPackageInfoCollection : KeyedCollection<string, PlatformPackageInfo> {
		protected override string GetKeyForItem(PlatformPackageInfo item) {
			ThrowHelper.ThrowIfNull(item);
			return item.Name;
		}
	}
	public record PlatformPackageInfo(
		string Name,
		VersionInfo LatestVersion,
		IList<string>? Versions
	);

	public record VersionInfo(
		string Name,
		IDictionary<string, LocalizedMetadataValueCollection> Metadata,
		IList<ResourceInfo>? Resources,
		DependencyInfoCollection? Dependencies,
		ISet<string>? PackedDependencies
	);

	public class LocalizedMetadataValueCollection : KeyedCollection<string, LocalizedMetadataValue> {
		protected override string GetKeyForItem(LocalizedMetadataValue item) {
			ThrowHelper.ThrowIfNull(item);
			return item.Culture;
		}
	}
	public record LocalizedMetadataValue(
		string Culture,
		string Value
	);

	public class DependencyInfoCollection : KeyedCollection<string, DependencyInfo> {
		protected override string GetKeyForItem(DependencyInfo item) {
			ThrowHelper.ThrowIfNull(item);
			return item.Id;
		}
	}
	public record DependencyInfo(
		string Id,
		string Version
	);
}
