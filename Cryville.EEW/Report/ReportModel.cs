using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Cryville.EEW.Report {
	/// <summary>
	/// Represents a report of an event.
	/// </summary>
	public sealed record ReportModel {
		/// <summary>
		/// The title of the report.
		/// </summary>
		public string? Title { get; set; }
		/// <summary>
		/// The source of the report.
		/// </summary>
		public string? Source { get; set; }
		/// <summary>
		/// The location of the event.
		/// </summary>
		public string? Location { get; set; }
		/// <summary>
		/// The specificity of the location described by <see cref="Location" />.
		/// </summary>
		/// <remarks>
		/// <para>This value conforms to the values of the <see href="https://wiki.openstreetmap.org/wiki/Key:admin_level"><c>admin_level</c> key in OpenStreetMap</see>, with the following extented values.</para>
		/// <list type="table">
		/// <item><term>0</term><description>Unknown specificity or unknown location</description></item>
		/// <item><term>12</term><description>A specific point that is inside the smallest possible local administrative region</description></item>
		/// </list>
		/// <para>Use the value 3 for a location that conforms to F-E regionalization.</para>
		/// </remarks>
		public int LocationSpecificity { get; set; }
		/// <summary>
		/// The predicate of the report.
		/// </summary>
		public string? Predicate { get; set; }
		/// <summary>
		/// The time of the event.
		/// </summary>
		public DateTimeOffset? Time { get; set; }
		/// <summary>
		/// The time when the report is to be invalidated.
		/// </summary>
		public DateTimeOffset? InvalidatedTime { get; set; }
		/// <summary>
		/// The time zone of the source.
		/// </summary>
		public TimeZoneInfo? TimeZone { get; set; }
		/// <summary>
		/// The time when the report is issued, in UTC.
		/// </summary>
		public DateTime UtcIssueTime { get; set; } = DateTime.UtcNow;
		/// <summary>
		/// The properties of the event.
		/// </summary>
		public IList<ReportProperty> Properties { get; } = [];
		/// <summary>
		/// Whether the report should be excluded from the history list.
		/// </summary>
		public bool IsExcludedFromHistory { get; set; }

		readonly ReportGroupKeyCollection m_groupKeys = [];
		/// <summary>
		/// The group keys of the report.
		/// </summary>
		public ICollection<IReportGroupKey> GroupKeys => m_groupKeys;
		/// <summary>
		/// The unit keys of the report.
		/// </summary>
		/// <remarks>
		/// This collection is read-only. Add unit keys by calling the <see cref="ICollection{T}.Add(T)" /> method on <see cref="GroupKeys" />.
		/// </remarks>
		public IReadOnlyCollection<IReportUnitKey> UnitKeys => m_groupKeys.UnitKeys;
		/// <summary>
		/// The revision key of the report.
		/// </summary>
		public IReportRevisionKey? RevisionKey { get; set; }

		/// <summary>
		/// The culture of the report.
		/// </summary>
		public CultureInfo Culture { get; internal set; } = CultureInfo.InvariantCulture;
		/// <summary>
		/// The original data model of the report.
		/// </summary>
		/// <remarks>
		/// It is not necessary to set this property in the generator if the model is the input model.
		/// </remarks>
		public object? Model { get; set; }

		sealed class ReportGroupKeyCollection : Collection<IReportGroupKey> {
			public readonly Collection<IReportUnitKey> UnitKeys = [];
			protected override void ClearItems() {
				base.ClearItems();
				UnitKeys.Clear();
			}
			protected override void InsertItem(int index, IReportGroupKey item) {
				base.InsertItem(index, item);
				if (item is IReportUnitKey unitKey) {
					UnitKeys.Add(unitKey);
				}
			}
			protected override void RemoveItem(int index) {
				var item = this[index];
				base.RemoveItem(index);
				if (item is IReportUnitKey unitKey) {
					UnitKeys.Remove(unitKey);
				}
			}
			protected override void SetItem(int index, IReportGroupKey item) {
				var oldItem = this[index];
				if (oldItem is IReportUnitKey oldUnitKey) {
					UnitKeys.Remove(oldUnitKey);
				}
				base.SetItem(index, item);
				if (item is IReportUnitKey unitKey) {
					UnitKeys.Add(unitKey);
				}
			}
		}
	}
}
