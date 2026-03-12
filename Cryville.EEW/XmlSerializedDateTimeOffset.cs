using Cryville.Common.Compat;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Cryville.EEW {
	/// <summary>
	/// A <see cref="DateTimeOffset" /> serialized in XML.
	/// </summary>
	public record struct XmlSerializedDateTimeOffset : IXmlSerializable, IComparable<XmlSerializedDateTimeOffset>, IComparable, IFormattable {
		/// <summary>
		/// The date time offset value.
		/// </summary>
		public DateTimeOffset Value { get; set; }

		/// <summary>
		/// Creates an instance of the <see cref="XmlSerializedDateTimeOffset" /> struct.
		/// </summary>
		/// <param name="value">The date time offset value.</param>
		public XmlSerializedDateTimeOffset(DateTimeOffset value) {
			Value = value;
		}

		/// <inheritdoc />
		public readonly XmlSchema? GetSchema() => null;
		/// <inheritdoc />
		public void ReadXml(XmlReader reader) {
			ThrowHelper.ThrowIfNull(reader);
			Value = XmlConvert.ToDateTimeOffset(reader.ReadElementContentAsString());
		}
		/// <inheritdoc />
		public readonly void WriteXml(XmlWriter writer) {
			ThrowHelper.ThrowIfNull(writer);
			writer.WriteString(Value.ToString("O", CultureInfo.InvariantCulture));
		}

		/// <inheritdoc />
		public readonly int CompareTo(XmlSerializedDateTimeOffset other) => Value.CompareTo(other.Value);
		/// <inheritdoc />
		public readonly int CompareTo(object? obj) => obj switch {
			null => 1,
			XmlSerializedDateTimeOffset other => Value.CompareTo(other.Value),
			DateTimeOffset other => Value.CompareTo(other),
			_ => throw new ArgumentException("Object must be of type XmlSerializedDateTimeOffset or DateTimeOffset."),
		};
		/// <inheritdoc />
		public static bool operator <(XmlSerializedDateTimeOffset left, XmlSerializedDateTimeOffset right) => left.CompareTo(right) < 0;
		/// <inheritdoc />
		public static bool operator <=(XmlSerializedDateTimeOffset left, XmlSerializedDateTimeOffset right) => left.CompareTo(right) <= 0;
		/// <inheritdoc />
		public static bool operator >(XmlSerializedDateTimeOffset left, XmlSerializedDateTimeOffset right) => left.CompareTo(right) > 0;
		/// <inheritdoc />
		public static bool operator >=(XmlSerializedDateTimeOffset left, XmlSerializedDateTimeOffset right) => left.CompareTo(right) >= 0;

		/// <inheritdoc />
		[SuppressMessage("CodeQuality", "IDE0079", Justification = "False report")]
		[SuppressMessage("Globalization", "CA1305", Justification = "Bypass")]
		[SuppressMessage("Globalization", "CRYVEEW0002", Justification = "Bypass")]
		public override readonly string ToString() => Value.ToString();
		/// <inheritdoc />
		[SuppressMessage("CodeQuality", "IDE0079", Justification = "False report")]
		[SuppressMessage("Globalization", "CA1305", Justification = "Bypass")]
		[SuppressMessage("Globalization", "CRYVEEW0002", Justification = "Bypass")]
		public readonly string ToString(string? format) => Value.ToString(format);
		/// <inheritdoc />
		public readonly string ToString(IFormatProvider? formatProvider) => Value.ToString(formatProvider);
		/// <inheritdoc />
		public readonly string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString(format, formatProvider);

		/// <summary>
		/// Converts an instance of the <see cref="DateTimeOffset" /> struct to an instance of the <see cref="XmlSerializedDateTimeOffset" /> struct.
		/// </summary>
		/// <param name="value">An instance of the <see cref="DateTimeOffset" /> struct.</param>
		/// <returns>An instance of the <see cref="XmlSerializedDateTimeOffset" /> struct.</returns>
		public static XmlSerializedDateTimeOffset FromDateTimeOffset(DateTimeOffset value) => new(value);
		/// <summary>
		/// Converts an instance of the <see cref="DateTimeOffset" /> struct to an instance of the <see cref="XmlSerializedDateTimeOffset" /> struct.
		/// </summary>
		/// <param name="value">An instance of the <see cref="DateTimeOffset" /> struct.</param>
		public static implicit operator XmlSerializedDateTimeOffset(DateTimeOffset value) => FromDateTimeOffset(value);
		/// <summary>
		/// Converts the current instance to an instance of the <see cref="DateTimeOffset" /> struct.
		/// </summary>
		/// <returns>An instance of the <see cref="DateTimeOffset" /> struct.</returns>
		public readonly DateTimeOffset ToDateTimeOffset() => Value;
		/// <summary>
		/// Converts an instance of the <see cref="XmlSerializedDateTimeOffset" /> struct to an instance of the <see cref="DateTimeOffset" /> struct.
		/// </summary>
		/// <param name="value">An instance of the <see cref="XmlSerializedDateTimeOffset" /> struct.</param>
		public static implicit operator DateTimeOffset(XmlSerializedDateTimeOffset value) => value.ToDateTimeOffset();
	}
}
