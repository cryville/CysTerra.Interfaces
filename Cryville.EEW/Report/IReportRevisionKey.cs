using System;
using System.Diagnostics.CodeAnalysis;

namespace Cryville.EEW.Report {
	/// <summary>
	/// Represents a key identifying the revision of a report.
	/// </summary>
	/// <remarks>
	/// <para>Implement <see cref="IComparable{IReportRevisionKey}.CompareTo(IReportRevisionKey)" /> to determine the precedence of the revision keys. By default, the precedence is determined by the following logic.</para>
	/// <list type="number">
	/// <item>Revision keys with <see cref="IsCancellation" /> being <see langword="true" /> is the latest revision.</item>
	/// <item>Otherwise, revision keys with a non-<see langword="null" /> <see cref="Serial"/> is the latest revision.</item>
	/// <item>Otherwise, revision keys with a greater <see cref="Serial"/> is the latest revision.</item>
	/// </list>
	/// </remarks>
	public interface IReportRevisionKey : IComparable<IReportRevisionKey> {
		/// <summary>
		/// The serial number of the revision.
		/// </summary>
		int? Serial => null;
		/// <summary>
		/// Whether the revision is the final revision.
		/// </summary>
		bool IsFinalRevision => false;
		/// <summary>
		/// Whether the revision is for cancellation.
		/// </summary>
		bool IsCancellation => false;
		/// <summary>
		/// Determines whether the revision key is comparable with the specified revision key.
		/// </summary>
		/// <param name="other">The other revision key.</param>
		/// <returns>Whether the revision key is comparable with the specified revision key.</returns>
		bool IsComparableWith(IReportRevisionKey other) => GetType() == other?.GetType();

		[SuppressMessage("CodeQuality", "IDE0079", Justification = "False report")]
		[SuppressMessage("Design", "CA1033", Justification = "Forward to IComparable")]
		int IComparable<IReportRevisionKey>.CompareTo(IReportRevisionKey? other) {
			if (IsCancellation) return other?.IsCancellation ?? false ? 0 : 1;
			if (other?.IsCancellation ?? false) return -1;
			if (Serial == null) return other?.Serial == null ? 0 : -1;
			if (other?.Serial == null) return 1;
			return Serial.Value.CompareTo(other.Serial.Value);
		}
	}
}
