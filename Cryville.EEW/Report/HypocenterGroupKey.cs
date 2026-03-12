using System;

namespace Cryville.EEW.Report {
	/// <summary>
	/// A group key based on hypocenter.
	/// </summary>
	/// <param name="Latitude">The latitude of the hypocenter.</param>
	/// <param name="Longitude">The longitude of the hypocenter.</param>
	/// <param name="DateTime">The origin date time in UTC.</param>
	/// <param name="Magnitude">The magnitude.</param>
	/// <param name="Depth">The depth in kilometers.</param>
	public record HypocenterGroupKey(double Latitude, double Longitude, DateTime DateTime, double Magnitude, double? Depth = null) : ISortableReportGroupKey {
		/// <inheritdoc />
		public int CompareTo(ISortableReportGroupKey obj) {
			if (obj is not HypocenterGroupKey other) throw new ArgumentException("Mismatched type.", nameof(obj));
			return DateTime.CompareTo(other.DateTime);
		}

		/// <inheritdoc />
		public bool PreMatch(ISortableReportGroupKey obj) {
			if (obj is not HypocenterGroupKey other) return false;
			return Math.Abs((DateTime - other.DateTime).TotalSeconds) <= 60;
		}

		/// <inheritdoc />
		public bool Match(ISortableReportGroupKey obj) {
			if (obj is not HypocenterGroupKey other) return false;
			var dtime = Math.Abs((DateTime - other.DateTime).TotalSeconds);
			if (dtime >= 60) return false;
			var dloc = Math.Abs(GeoUtils.GreatCircleDistance(Latitude, Longitude, other.Latitude, other.Longitude));
			if (dloc >= 1.5 * Math.PI / 180.0) return false;
			var mdloc = dloc / (105.0 / 6379.0);
			var mdtime = dtime / 13.0;
			var mdmag = Math.Abs(Magnitude - other.Magnitude) / 0.8;
			return (mdloc < 1.2 ? 1 : 0) + (mdtime < 1.2 ? 1 : 0) + (mdmag < 1.2 ? 1 : 0) >= 2;
		}
	}
}
