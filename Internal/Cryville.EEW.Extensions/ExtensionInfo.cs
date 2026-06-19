using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Cryville.EEW.Extensions {
	[method: JsonConstructor]
	public sealed record ExtensionInfo(
		string AssemblyName,
		Version? Version,
		byte[] Hash
	) {
		public ExtensionInfo(
			string AssemblyName,
			string DisplayName,
			CultureInfo DisplayNameCulture,
			string? Description,
			CultureInfo? DescriptionCulture,
			Version? Version,
			byte[] Hash,
			IEnumerable<AssemblyName> ReferencedAssemblies
		) : this(AssemblyName, Version, Hash) {
			this.DisplayName = DisplayName;
			this.DisplayNameCulture = DisplayNameCulture;
			this.Description = Description;
			this.DescriptionCulture = DescriptionCulture;
			this.ReferencedAssemblies = ReferencedAssemblies;
		}
		[JsonIgnore]
		public string DisplayName { get; } = AssemblyName;
		[JsonIgnore]
		public CultureInfo DisplayNameCulture { get; } = CultureInfo.InvariantCulture;
		[JsonIgnore]
		public string? Description { get; }
		[JsonIgnore]
		public CultureInfo? DescriptionCulture { get; }
		[JsonIgnore]
		public IEnumerable<AssemblyName> ReferencedAssemblies { get; } = [];
		[JsonIgnore]
		public bool HasExplicitDisplayName => DisplayName != AssemblyName;
	}
}
