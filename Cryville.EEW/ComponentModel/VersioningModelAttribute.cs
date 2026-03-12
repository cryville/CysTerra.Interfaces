using System;

namespace Cryville.EEW.ComponentModel {
	/// <summary>
	/// Indicates the versioning model the assembly is using.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public abstract class VersioningModelAttribute : Attribute {
		/// <summary>
		/// Determines whether the public interface of the assembly is compatible when updating from <paramref name="fromVersion" /> to <paramref name="toVersion" />.
		/// </summary>
		/// <param name="fromVersion">The version updating from.</param>
		/// <param name="toVersion">The version updating to.</param>
		/// <returns>Whether the public interface of the assembly is compatible when updating from <paramref name="fromVersion" /> to <paramref name="toVersion" />.</returns>
		public abstract bool IsCompatible(Version fromVersion, Version toVersion);
	}
}
