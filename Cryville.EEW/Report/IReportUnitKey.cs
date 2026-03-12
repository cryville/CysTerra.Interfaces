namespace Cryville.EEW.Report {
	/// <summary>
	/// Represents a key for identifying report units.
	/// </summary>
	public interface IReportUnitKey : IReportGroupKey {
		/// <summary>
		/// Determines whether the report of the current unit key is covered by another report.
		/// </summary>
		/// <param name="key">The unit key of the other report.</param>
		/// <returns>Whether the report of the current unit key is covered by the other report.</returns>
		bool IsCoveredBy(IReportUnitKey key) => Equals(key);
	}
}
