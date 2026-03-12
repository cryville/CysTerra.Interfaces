using Cryville.Common.Compat;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cryville.EEW {
	/// <summary>
	/// Converts instances of the <see cref="DateTime" /> struct to or from JSON.
	/// </summary>
	public class NonstandardDateTimeJsonConverter : JsonConverter<DateTime> {
		/// <inheritdoc />
		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
			Debug.Assert(typeToConvert == typeof(DateTime));
			var value = reader.GetString();
			if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
				throw new JsonException(null, new FormatException("Invalid DateTime format."));
			return result;
		}

		/// <inheritdoc />
		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) {
			ThrowHelper.ThrowIfNull(writer);
			writer.WriteStringValue(value.ToString("O", CultureInfo.InvariantCulture));
		}
	}
}
