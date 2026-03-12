using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Cryville.EEW.Features {
	/// <summary>
	/// Represents a geometry in coordinate space.
	/// </summary>
	public abstract record Geometry;
	/// <summary>
	/// Represents an unknown geometry.
	/// </summary>
	public record UnknownGeometry : Geometry {
		static UnknownGeometry? s_instance;
		/// <summary>
		/// A shared instance of the <see cref="UnknownGeometry" /> class.
		/// </summary>
		public static UnknownGeometry Instance => s_instance ??= new();
	}
	/// <summary>
	/// Represents a point.
	/// </summary>
	/// <param name="Coordinates">The coordinates, a single position.</param>
	public record Point(Coordinates Coordinates) : Geometry {
		/// <summary>
		/// Creates an instance of the <see cref="Point" /> class.
		/// </summary>
		/// <param name="longitude">The longitude in degrees.</param>
		/// <param name="latitude">The latitude in degrees.</param>
		public Point(double longitude, double latitude) : this(new Coordinates(longitude, latitude)) { }
	}
	/// <summary>
	/// Represents multiple points.
	/// </summary>
	/// <param name="Coordinates">The coordinates, an array of positions.</param>
	public record MultiPoint(IEnumerable<Coordinates> Coordinates) : Geometry;
	/// <summary>
	/// Represents a path geometry.
	/// </summary>
	public abstract record PathGeometry : Geometry;
	/// <summary>
	/// Represents a line.
	/// </summary>
	/// <param name="Coordinates">The coordinates, an array of two or more positions.</param>
	public record LineString(IEnumerable<Coordinates> Coordinates) : PathGeometry;
	/// <summary>
	/// Represents multiple lines.
	/// </summary>
	/// <param name="Coordinates">The coordinates, an array of <see cref="LineString.Coordinates" /> arrays.</param>
	public record MultiLineString(IEnumerable<IEnumerable<Coordinates>> Coordinates) : PathGeometry;
	/// <summary>
	/// Represents a polygon.
	/// </summary>
	/// <param name="Coordinates">The coordinates, an array of linear ring coordinate arrays.</param>
	public record Polygon(IEnumerable<IEnumerable<Coordinates>> Coordinates) : PathGeometry;
	/// <summary>
	/// Represents multiple polygons.
	/// </summary>
	/// <param name="Coordinates">The coordinates, an array of <see cref="Polygon.Coordinates" /> arrays.</param>
	public record MultiPolygon(IEnumerable<IEnumerable<IEnumerable<Coordinates>>> Coordinates) : PathGeometry;
	/// <summary>
	/// Represents a geometry collection.
	/// </summary>
	/// <param name="Geometries">An array of <see cref="Geometry" /> objects.</param>
	[SuppressMessage("CodeQuality", "IDE0079", Justification = "False report")]
	[SuppressMessage("Naming", "CA1711", Justification = "[sic]")]
	public record GeometryCollection(IEnumerable<Geometry> Geometries) : Geometry;
}
