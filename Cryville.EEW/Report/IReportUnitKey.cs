namespace Cryville.EEW.Report {
	/// <summary>
	/// Represents a key for identifying report units.
	/// </summary>
	/// <remarks>
	/// <para>As <see cref="IReportGroupKey" />, unit keys are also matched with the <see cref="object.Equals(object)" /> method and hashed with the <see cref="object.GetHashCode" /> method.</para>
	/// <para>It is recommended to use the <see langword="record" /> type to implement this interface because it automatically implements the aforementioned two methods.</para>
	/// </remarks>
	public interface IReportUnitKey : IReportGroupKey {
		/// <summary>
		/// Determines whether the report of the current unit key is covered by another report.
		/// </summary>
		/// <param name="key">The unit key of the other report.</param>
		/// <returns>Whether the report of the current unit key is covered by the other report.</returns>
		/// <remarks>
		/// <para>Unit keys are matched with the <see cref="object.Equals(object)" /> method instead of this method.</para>
		/// <para>A report, if covered by another report, may be considered insignificant and invalidated.</para>
		/// </remarks>
		bool IsCoveredBy(IReportUnitKey key) => Equals(key);
	}
}
