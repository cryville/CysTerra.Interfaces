using System;
using System.ComponentModel;

namespace Cryville.EEW.ComponentModel {
	/// <summary>
	/// Indicates that the attributed assembly uses extended <see href="https://semver.org/">semantic versioning</see>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class UsesExtendedSemanticVersioningAttribute : VersioningModelAttribute {
		/// <inheritdoc />
		public override bool IsCompatible(Version fromVersion, Version toVersion) {
			if (fromVersion == toVersion)
				return true;
			if (toVersion < fromVersion)
				return false;
			if (fromVersion.Major == 0) {
				if (toVersion.Major > 0)
					return false;
				return toVersion < new Version(0, fromVersion.Minor + 1);
			}
			return toVersion < new Version(fromVersion.Major + 1, 0);
		}
	}
}
