namespace Cryville.EEW.Features {
	/// <summary>
	/// The fundamental geometry construct.
	/// </summary>
	/// <param name="Longitude">The longitude in degrees.</param>
	/// <param name="Latitude">The latitude in degrees.</param>
	public record struct Coordinates(double Longitude, double Latitude);
}
