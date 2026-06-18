using System;

namespace Cryville.Packages {
	public class PackageNotResolvedException : InvalidOperationException {
		private const string DEFAULT_MESSAGE = "Cannot resolve the specified package.";

		public string? Package { get; }
		public string? Version { get; }
		public PackageNotResolvedReason? Reason { get; }

		public PackageNotResolvedException() : base(DEFAULT_MESSAGE) { }
		public PackageNotResolvedException(string message) : base(message) { }
		public PackageNotResolvedException(string message, Exception innerException) : base(message, innerException) { }

		public PackageNotResolvedException(string package, string? version, PackageNotResolvedReason reason) : this(package, version, reason, null) { }
		public PackageNotResolvedException(string package, string? version, PackageNotResolvedReason reason, Exception? innerException) : base(DEFAULT_MESSAGE, innerException) {
			Package = package;
			Version = version;
			Reason = reason;
		}
	}
	public enum PackageNotResolvedReason {
		ResourceNotFound,
		VersionInfoNotFound,
	}
}
