using System;
using System.Collections.Generic;
using System.Globalization;

namespace Cryville.EEW.Report {
	/// <summary>
	/// Represents a report property.
	/// </summary>
	/// <param name="Type">The type.</param>
	/// <param name="RawValue">The raw value.</param>
	/// <param name="Key">The name.</param>
	/// <param name="Value">The value.</param>
	/// <param name="Severity">The severity</param>
	public sealed record ReportProperty(TagTypeKey Type, object? RawValue, string? Key, string Value, float Severity) : TaggedProperty(new(Type, RawValue), Key, Value, CultureInfo.InvariantCulture) {
		readonly float? _severityForColorHint;

		/// <summary>
		/// Creates an instance of the <see cref="ReportProperty" /> class.
		/// </summary>
		/// <param name="Type">The type.</param>
		/// <param name="Key">The name.</param>
		/// <param name="Value">The value.</param>
		/// <param name="Severity">The severity</param>
		public ReportProperty(TagTypeKey Type, string? Key, string Value, float Severity)
			: this(Type, null, Key, Value, Severity) { _severityForColorHint = Severity; }

		/// <summary>
		/// Creates an instance of the <see cref="ReportProperty" /> class.
		/// </summary>
		/// <param name="Type">The type.</param>
		/// <param name="Key">The name.</param>
		/// <param name="Value">The value.</param>
		/// <param name="severityScheme">The severity scheme.</param>
		/// <param name="rawValue">The raw value.</param>
		public ReportProperty(TagTypeKey Type, string? Key, string Value, ISeverityScheme severityScheme, object? rawValue)
			: this(Type, rawValue, Key, Value, severityScheme?.From(Type, rawValue) ?? throw new ArgumentNullException(nameof(severityScheme))) { }

		/// <summary>
		/// The accuracy of the value.
		/// </summary>
		/// <remarks>
		/// <para>This value conforms to the following definitions.</para>
		/// <list type="table">
		/// <item><term>0~19 (10 by default)</term><description>Strict (observed or derived from observed data strictly based on the definition), reviewed</description></item>
		/// <item><term>20~39 (30 by default)</term><description>Strict (observed or derived from observed data strictly based on the definition), automatic</description></item>
		/// <item><term>40~59 (50 by default)</term><description>Estimated (estimated from observed data of the same type)</description></item>
		/// <item><term>60~79 (70 by default)</term><description>Forecast (estimated from observed data of different types)</description></item>
		/// <item><term>80~99 (90 by default)</term><description>Unofficial</description></item>
		/// <item><term>100</term><description>Unknown</description></item>
		/// </list>
		/// <para>The default value is 10.</para>
		/// </remarks>
		public int AccuracyOrder { get; init; } = 10;

		/// <inheritdoc />
		public override KeyValuePair<TagTypeKey, object?>? GetTagForColorHint() =>
			_severityForColorHint is float severity ? new(SpecialTagTypeKeys.Severity, severity) : base.GetTagForColorHint();
	}
}
