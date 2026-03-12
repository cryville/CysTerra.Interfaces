using System.Diagnostics.CodeAnalysis;

namespace Cryville.EEW.Report {
	/// <summary>
	/// Represents a key for grouping reports.
	/// </summary>
	[SuppressMessage("CodeQuality", "IDE0079", Justification = "False report")]
	[SuppressMessage("Design", "CA1040", Justification = "Used for type identification")]
	public interface IReportGroupKey { }

	/// <summary>
	/// Represents a sortable report group key.
	/// </summary>
	public interface ISortableReportGroupKey : IReportGroupKey {
		/// <summary>
		/// Compares the group key with another group key and returns an integer that indicates whether the current group key precedes, follows, or occurs in the same position in the sort order as the other group key.
		/// </summary>
		/// <param name="obj">The other group key.</param>
		/// <returns>A value that indicates the relative order of the group keys being compared. Negative if the current group key precedes <paramref name="obj" />, zero if they occurs in the same position, or positive if the current group key follows <paramref name="obj" />.</returns>
		/// <remarks>
		/// For any group key, among a list where all the group keys are sorted with this method, there must be at most one consecutive subsequence where all the groups keys pre-match with that group key, and all the other group keys in the list must not pre-match with that group key.
		/// </remarks>
		int CompareTo(ISortableReportGroupKey obj);
		/// <summary>
		/// Pre-matches against another group key.
		/// </summary>
		/// <param name="obj">The other group key.</param>
		/// <returns>Whether the two group keys pre-match, to potentially actually match.</returns>
		/// <remarks>
		/// <para>Implement this method so that it returns only based on the conditions used in <see cref="CompareTo(ISortableReportGroupKey)" />. For example, if <see cref="CompareTo(ISortableReportGroupKey)" /> only takes the event time into account, then implement this method so that it also only takes the event time into account, returns <see langword="true" /> if the two group keys potentially match, and returns <see langword="false" /> if they can never match.</para>
		/// <para>Among a list where all the group keys are sorted with <see cref="CompareTo(ISortableReportGroupKey)" />, there must be at most one consecutive subsequence where all the groups keys pre-match with this group key, and all the other group keys in the list must not pre-match with this group key.</para>
		/// </remarks>
		bool PreMatch(ISortableReportGroupKey obj);
		/// <summary>
		/// Matches against another group key.
		/// </summary>
		/// <param name="obj">The other group key.</param>
		/// <returns>Whether the two group keys match.</returns>
		/// <remarks>
		/// <para>Report groups with any matching group keys are grouped together.</para>
		/// </remarks>
		bool Match(ISortableReportGroupKey obj);
	}
}
