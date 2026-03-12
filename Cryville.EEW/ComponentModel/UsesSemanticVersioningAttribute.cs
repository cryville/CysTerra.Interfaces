using System;

namespace Cryville.EEW.ComponentModel {
	/// <summary>
	/// Indicates that the attributed assembly uses <see href="https://semver.org/">semantic versioning</see>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class UsesSemanticVersioningAttribute : VersioningModelAttribute {
		/// <inheritdoc />
		public override bool IsCompatible(Version fromVersion, Version toVersion) {
			if (fromVersion == toVersion)
				return true;
			if (toVersion < fromVersion)
				return false;
			if (fromVersion.Major == 0)
				return false;
			return toVersion < new Version(fromVersion.Major + 1, 0);
		}
	}
}
