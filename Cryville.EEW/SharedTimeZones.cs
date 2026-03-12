using Cryville.Common.Compat;
using System;
using System.Globalization;

namespace Cryville.EEW {
	/// <summary>
	/// Shared time zones.
	/// </summary>
	public static class SharedTimeZones {
		/// <summary>
		/// Gets a time zone.
		/// </summary>
		/// <param name="id">The ID of the time zone.</param>
		/// <param name="fallbackOffset">The time zone offset to be used as a fallback when <paramref name="id" /> is not found.</param>
		/// <returns></returns>
		public static TimeZoneInfo GetTimeZone(string id, TimeSpan fallbackOffset) {
			try {
				return TimeZoneInfo.FindSystemTimeZoneById(id);
			}
			catch (TimeZoneNotFoundException) {
				return TimeZoneInfo.CreateCustomTimeZone(id, fallbackOffset, null, null);
			}
		}
		/// <summary>
		/// Converts a time zone to a string representation.
		/// </summary>
		/// <param name="tz">The time zone to be converted.</param>
		/// <param name="time">The date time offset.</param>
		/// <returns>A string representation of the time zone.</returns>
		public static string ToTimeZoneString(this TimeZoneInfo tz, DateTimeOffset time) {
			ThrowHelper.ThrowIfNull(tz);
			return tz.GetUtcOffset(time).ToOffsetString();
		}
		/// <summary>
		/// Converts a time span as an offset to a string representation.
		/// </summary>
		/// <param name="offset">The time span to be converted.</param>
		/// <returns>A string representation of the time span as an offset.</returns>
		public static string ToOffsetString(this TimeSpan offset) {
			if (offset == TimeSpan.Zero) return "UTC";
			else if (offset < TimeSpan.Zero) return offset.ToString("'UTC-'hh':'mm", CultureInfo.InvariantCulture);
			else return offset.ToString("'UTC+'hh':'mm", CultureInfo.InvariantCulture);
		}
	}
}
