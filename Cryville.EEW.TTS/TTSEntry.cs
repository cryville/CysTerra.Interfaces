using Cryville.EEW.Report;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Cryville.EEW.TTS {
	/// <summary>
	/// Represents an entry sent to a TTS worker.
	/// </summary>
	/// <param name="Locale">The locale of the content.</param>
	/// <param name="Title">The title of the entry, if non-<see langword="null" />, used when the entry is being repeated after interrupted; <see langword="null" /> if the entry should not be repeated after interrupted.</param>
	/// <param name="Content">The content to be spoken.</param>
	/// <param name="Priority">The priority (a lower number indicates higher priority).</param>
	/// <param name="Sound">The sound to be played.</param>
	/// <param name="IsRepeated">Whether the entry is interrupted and to be repeated.</param>
	public sealed record TTSEntry(CultureInfo Locale, string? Title, string Content, int Priority, string? Sound = null, bool IsRepeated = false) {
		/// <summary>
		/// The priority (a lower number indicates higher priority).
		/// </summary>
		public int Priority { get; internal set; } = Priority;
		/// <summary>
		/// Copied from <see cref="ReportModel.UnitKeys" />.
		/// </summary>
		public IReadOnlyCollection<IReportUnitKey>? UnitKeys { get; internal set; }
		/// <summary>
		/// Copied from <see cref="ReportModel.RevisionKey" />.
		/// </summary>
		public IReportRevisionKey? RevisionKey { get; internal set; }
		/// <summary>
		/// The sound to be played.
		/// </summary>
		public string? Sound { get; set; } = Sound;
		/// <summary>
		/// The TTS entry to be inserted into the queue before this TTS entry is inserted.
		/// </summary>
		public TTSEntry? UrgentEntry { get; set; }
		/// <summary>
		/// The TTS entry to be inserted into the queue when this TTS entry starts being spoken.
		/// </summary>
		public TTSEntry? SecondaryEntry { get; internal set; }
		/// <summary>
		/// The time when the entry is created in UTC.
		/// </summary>
		public DateTime IssueTime { get; set; } = DateTime.UtcNow;
	}
}
