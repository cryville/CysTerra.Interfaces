using System;

namespace Cryville.EEW {
	/// <summary>
	/// Provides <see langword="static" /> utility methods related to geography.
	/// </summary>
	public static class GeoUtils {
		/// <summary>
		/// Gets the great circle distance between two locations.
		/// </summary>
		/// <param name="lat1">The latitude of the first location.</param>
		/// <param name="lon1">The longitude of the first location.</param>
		/// <param name="lat2">The latitude of the second location.</param>
		/// <param name="lon2">The longitude of the second location.</param>
		/// <returns>The great circle distance between the two locations in radians.</returns>
		public static double GreatCircleDistance(double lat1, double lon1, double lat2, double lon2) {
			lat1 *= Math.PI / 180;
			lon1 *= Math.PI / 180;
			lat2 *= Math.PI / 180;
			lon2 *= Math.PI / 180;
			double slat = Math.Sin((lat2 - lat1) / 2);
			double slon = Math.Sin((lon2 - lon1) / 2);
			double a = slat * slat + Math.Cos(lat1) * Math.Cos(lat2) * slon * slon;
			return 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
		}
	}
}
