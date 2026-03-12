namespace Cryville.EEW {
	/// <summary>
	/// Provides a set of stable instances of <see cref="TagTypeKey" /> that are well defined.
	/// </summary>
	public static class TagTypeKeys {
		/// <inheritdoc cref="SpecialTagTypeKeys.Is" />
		public static readonly TagTypeKey Is = SpecialTagTypeKeys.Is;
		/// <inheritdoc cref="SpecialTagTypeKeys.Includes" />
		public static readonly TagTypeKey Includes = SpecialTagTypeKeys.Includes;

		#region Substantial tags
		/// <summary>
		/// Used as the value of <see cref="Is" />. Represents a geological feature.
		/// </summary>
		/// <remarks>
		/// Defined by OSM: <see href="https://wiki.openstreetmap.org/wiki/Key:geological"><c>geological=*</c></see>.
		/// <para>This tag is a parent type, and a subtype of this type should be used instead.</para>
		/// </remarks>
		public static readonly TagTypeKey Geological = "geological";
		/// <summary>
		/// Used as the value of <see cref="Is" />. Represents a hole through which the lava erupts.
		/// </summary>
		/// <remarks>
		/// Defined by OSM: <see href="https://wiki.openstreetmap.org/wiki/Tag%3Ageological%3Dvolcanic_vent"><c>geological=volcanic_vent</c></see>.
		/// </remarks>
		public static readonly TagTypeKey GeologicalVolcanicVent = Geological.OfSubtype("volcanic_vent");
		/// <summary>
		/// Used as the value of <see cref="Is" />. Represents a man-made (artificial) structures added to the landscape.
		/// </summary>
		/// <remarks>
		/// Defined by OSM: <see href="https://wiki.openstreetmap.org/wiki/Key:man_made"><c>man_made=*</c></see>.
		/// <para>This tag is a parent type, and a subtype of this type should be used instead.</para>
		/// </remarks>
		public static readonly TagTypeKey ManMade = "man_made";
		/// <summary>
		/// Used as the value of <see cref="Is" />. Represents a monitoring station.
		/// </summary>
		/// <remarks>
		/// Defined by OSM: <see href="https://wiki.openstreetmap.org/wiki/Tag:man_made%3Dmonitoring_station"><c>man_made=monitoring_station</c></see>.
		/// <para>Use <see cref="Monitoring" /> to specify what properties or subjects are being monitored.</para>
		/// </remarks>
		public static readonly TagTypeKey ManMadeMonitoringStation = ManMade.OfSubtype("monitoring_station");
		/// <summary>
		/// Used as the value of <see cref="Is" />. Represents a physical geography, geological or landcover feature.
		/// </summary>
		/// <remarks>
		/// Defined by OSM: <see href="https://wiki.openstreetmap.org/wiki/Key:natural"><c>natural=*</c></see>.
		/// <para>This tag is a parent type, and a subtype of this type should be used instead.</para>
		/// </remarks>
		public static readonly TagTypeKey Natural = "natural";
		/// <summary>
		/// Used as the value of <see cref="Is" />. Represents a volcano.
		/// </summary>
		/// <remarks>
		/// Defined by OSM: <see href="https://wiki.openstreetmap.org/wiki/Tag:natural%3Dvolcano"><c>natural=volcano</c></see>.
		/// </remarks>
		public static readonly TagTypeKey NaturalVolcano = Natural.OfSubtype("volcano");
		/// <summary>
		/// Used as the value of <see cref="Is" />. Represents a particular location known by a particular name.
		/// </summary>
		/// <remarks>
		/// Defined by OSM: <see href="https://wiki.openstreetmap.org/wiki/Key:place"><c>place=*</c></see>.
		/// <para>This tag is a parent type, and a subtype of this type should be used instead.</para>
		/// </remarks>
		public static readonly TagTypeKey Place = "place";
		/// <summary>
		/// Used as the value of <see cref="Is" />. Represents an area against which information (warnings, advisories, messages, etc.) is issued.
		/// </summary>
		/// <remarks>
		/// <para>Use <see cref="AreaLevel" /> to specify the level of the area within its hierarchy.</para>
		/// <para>Use <see cref="Subject" /> to specify what kind of information is issued for the area.</para>
		/// </remarks>
		public static readonly TagTypeKey PlaceInfoArea = Place.OfSubtype("info_area");

		/// <summary>
		/// Describes the level of the area within its hierarchy.
		/// </summary>
		/// <remarks>
		/// The value should be an integer that roughly corresponds to the values of the <see href="https://wiki.openstreetmap.org/wiki/Key:admin_level"><c>admin_level</c> key in OpenStreetMap</see>.
		/// </remarks>
		public static readonly TagTypeKey AreaLevel = "area_level";
		/// <summary>
		/// Describes additional comments to the feature.
		/// </summary>
		/// <remarks>
		/// The value should be a (localized) string or an enumerable of (localized) strings.
		/// </remarks>
		public static readonly TagTypeKey Comment = "comment";
		/// <summary>
		/// Describes additional information about the related element.
		/// </summary>
		/// <remarks>
		/// Defined by OSM: <see href="https://wiki.openstreetmap.org/wiki/Key:description"><c>description=*</c></see>.
		/// <para>The value should be a (localized) string or an enumerable of (localized) strings.</para>
		/// </remarks>
		public static readonly TagTypeKey Description = "description";
		/// <summary>
		/// Describes the monitoring type.
		/// </summary>
		/// <remarks>
		/// Defined by OSM: <see href="https://wiki.openstreetmap.org/wiki/Key:monitoring:*"><c>monitoring:*=*</c></see>.
		/// <para>This tag is a parent type, and a subtype of this type should be used instead.</para>
		/// <para>The value should be a boolean specifying whether the property or subject is monitored.</para>
		/// </remarks>
		public static readonly TagTypeKey Monitoring = "monitoring";
		/// <summary>
		/// Describes whether seismic activity is monitored.
		/// </summary>
		/// <remarks>
		/// Defined by OSM: <see href="https://wiki.openstreetmap.org/wiki/Key:monitoring:seismic_activity"><c>monitoring:seismic_activity=*</c></see>.
		/// <para>The value should be a boolean specifying whether seismic activity is monitored.</para>
		/// </remarks>
		public static readonly TagTypeKey MonitoringSeismicActivity = Monitoring.OfSubtype("seismic_activity");
		/// <summary>
		/// Describes whether tide gauge is monitored.
		/// </summary>
		/// <remarks>
		/// Defined by OSM: <see href="https://wiki.openstreetmap.org/wiki/Key:monitoring:tide_gauge"><c>monitoring:tide_gauge=*</c></see>.
		/// <para>The value should be a boolean specifying whether tide gauge is monitored.</para>
		/// </remarks>
		public static readonly TagTypeKey MonitoringTideGauge = Monitoring.OfSubtype("tide_gauge");
		/// <summary>
		/// Describes the primary name of the feature.
		/// </summary>
		/// <remarks>
		/// Defined by OSM: <see href="https://wiki.openstreetmap.org/wiki/Key:name"><c>name=*</c></see>.
		/// <para>The value should be a (localized) string or an enumerable of (localized) strings.</para>
		/// </remarks>
		public static readonly TagTypeKey Name = "name";
		/// <summary>
		/// Describes the name of the company, corporation, person or any other entity who is directly in charge of the current operation of the feature.
		/// </summary>
		/// <remarks>
		/// Defined by OSM: <see href="https://wiki.openstreetmap.org/wiki/Key:operator"><c>operator=*</c></see>.
		/// <para>The value should be a (localized) string or an enumerable of (localized) strings.</para>
		/// </remarks>
		public static readonly TagTypeKey Operator = "operator";
		/// <summary>
		/// Describes a reference number or code.
		/// </summary>
		/// <remarks>
		/// Defined by OSM: <see href="https://wiki.openstreetmap.org/wiki/Key:ref"><c>ref=*</c></see>.
		/// <para>The value should be a string.</para>
		/// </remarks>
		public static readonly TagTypeKey Ref = "ref";
		/// <summary>
		/// Describes the source of the information.
		/// </summary>
		/// <remarks>
		/// Defined by OSM: <see href="https://wiki.openstreetmap.org/wiki/Key:source"><c>source=*</c></see>.
		/// <para>The value should be a (localized) string or an enumerable of (localized) strings.</para>
		/// </remarks>
		public static readonly TagTypeKey Source = "source";
		/// <summary>
		/// Describes the subject of the feature.
		/// </summary>
		/// <remarks>
		/// Defined by OSM: <see href="https://wiki.openstreetmap.org/wiki/Key:subject"><c>subject=*</c></see>.
		/// <para>The value should be a feature, an enumerable of features, or a <see cref="TagTypeKey" />.</para>
		/// </remarks>
		public static readonly TagTypeKey Subject = "subject";
		#endregion



		/// <inheritdoc cref="SpecialTagTypeKeys.At" />
		public static readonly TagTypeKey At = SpecialTagTypeKeys.At;

		#region Event tags
		/// <summary>
		/// Used as the value of <see cref="Is" />. Represents an earthquake event.
		/// </summary>
		public static readonly TagTypeKey Earthquake = "Earthquake";
		/// <summary>
		/// Used as the value of <see cref="Is" />. Represents a hypocenter of an earthquake event.
		/// </summary>
		public static readonly TagTypeKey Hypocenter = "Hypocenter";
		/// <summary>
		/// Used as the value of <see cref="Is" />. Represents an assumed hypocenter of an earthquake event.
		/// </summary>
		public static readonly TagTypeKey HypocenterAssumed = Hypocenter.OfSubtype("Assumed");
		/// <summary>
		/// Used as the value of <see cref="Is" />. Represents a report (forecast, observation, etc.) of an event.
		/// </summary>
		/// <remarks>
		/// <para>This tag is a parent type. A subtype of this type should be used if possible, but it may also be used directly if the report consists of different subtypes of reports.</para>
		/// </remarks>
		public static readonly TagTypeKey Report = "Report";
		/// <summary>
		/// Used as the value of <see cref="Is" />. Represents a forecast report of an event.
		/// </summary>
		public static readonly TagTypeKey ReportForecast = Report.OfSubtype("Forecast");
		/// <summary>
		/// Used as the value of <see cref="Is" />. Represents an observation report of an event.
		/// </summary>
		public static readonly TagTypeKey ReportObservation = Report.OfSubtype("Observation");
		/// <summary>
		/// Used as the value of <see cref="Is" />. Represents an tsunami event.
		/// </summary>
		public static readonly TagTypeKey Tsunami = "Tsunami";

		/// <inheritdoc cref="SpecialTagTypeKeys.Severity" />
		public static readonly TagTypeKey Severity = SpecialTagTypeKeys.Severity;

		/// <summary>
		/// Describes the depth of the hypocenter relative to the ground level.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a quantity. A number is also acceptable but not recommended, and when used, must be in kilometers.</para>
		/// <para>A positive value indicates a hypocenter under ground. A negative value indicates a hypocenter above ground.</para>
		/// </remarks>
		public static readonly TagTypeKey HypocenterDepth = "HypocenterDepth";

		/// <summary>
		/// Describes the seismic intensity.
		/// </summary>
		/// <remarks>
		/// <para>This tag is a parent type, and a subtype of this type should be used instead.</para>
		/// <para>When used on an area, the tag describes the maximum seismic intensity.</para>
		/// </remarks>
		public static readonly TagTypeKey Intensity = "Intensity";
		/// <summary>
		/// Describes the CWASIS (Central Weather Administration (Taiwan) seismic intensity scale) seismic intensity.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a string, one of <c>"1"</c>, <c>"2"</c>, <c>"3"</c>, <c>"4"</c>, <c>"5"</c>, <c>"5-"</c>, <c>"5+"</c>, <c>"6"</c>, <c>"6-"</c>, <c>"6+"</c>, and <c>"7"</c>.</para>
		/// <para>When used on an area, the tag describes the maximum seismic intensity.</para>
		/// </remarks>
		public static readonly TagTypeKey IntensityCWASIS = Intensity.OfSubtype("CWASIS");
		/// <summary>
		/// Describes the CSIS (China seismic intensity scale) seismic intensity.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a number.</para>
		/// <para>When used on an area, the tag describes the maximum seismic intensity.</para>
		/// </remarks>
		public static readonly TagTypeKey IntensityCSIS = Intensity.OfSubtype("CSIS");
		/// <summary>
		/// Describes the JMA (Japan Meteorological Agency) seismic intensity scale seismic intensity.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a string or a number. When it is a string, it should be one of <c>"0"</c>, <c>"1"</c>, <c>"2"</c>, <c>"3"</c>, <c>"4"</c>, <c>"5"</c>, <c>"5-"</c>, <c>"5+"</c>, <c>"6"</c>, <c>"6-"</c>, <c>"6+"</c>, and <c>"7"</c>.</para>
		/// <para>When used on an area, the tag describes the maximum seismic intensity.</para>
		/// </remarks>
		public static readonly TagTypeKey IntensityJMA = Intensity.OfSubtype("JMA");
		/// <summary>
		/// Describes the JMA (Japan Meteorological Agency) long-period seismic intensity class.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a string, one of <c>"1"</c>, <c>"2"</c>, <c>"3"</c>, and <c>"4"</c>. The value should NOT be a number.</para>
		/// <para>When used on an area, the tag describes the maximum seismic intensity.</para>
		/// </remarks>
		public static readonly TagTypeKey IntensityJMALPGM = Intensity.OfSubtype("JMALPGM");
		/// <summary>
		/// Describes the MMI (Modified Mercalli intensity scale) seismic intensity.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a number.</para>
		/// <para>When used on an area, the tag describes the maximum seismic intensity.</para>
		/// </remarks>
		public static readonly TagTypeKey IntensityMMI = Intensity.OfSubtype("MMI");

		/// <summary>
		/// Describes the seismic magnitude.
		/// </summary>
		/// <remarks>
		/// <para>This tag is a parent type. A subtype of this type should be used if possible, but it may also be used directly if the magnitude type is not specified or the subtype for the magnitude type is not defined.</para>
		/// <para>The value should be a number.</para>
		/// </remarks>
		public static readonly TagTypeKey Magnitude = "Magnitude";
		/// <summary>
		/// Describes the body-wave seismic magnitude.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a number.</para>
		/// </remarks>
		public static readonly TagTypeKey MagnitudeBodyWave = Magnitude.OfSubtype("BodyWave");
		/// <summary>
		/// Describes the duration seismic magnitude.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a number.</para>
		/// </remarks>
		public static readonly TagTypeKey MagnitudeDuration = Magnitude.OfSubtype("Duration");
		/// <summary>
		/// Describes the JMA (Japan Meteorological Agency) seismic magnitude.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a number.</para>
		/// </remarks>
		public static readonly TagTypeKey MagnitudeJMA = Magnitude.OfSubtype("JMA");
		/// <summary>
		/// Describes the local seismic magnitude.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a number.</para>
		/// </remarks>
		public static readonly TagTypeKey MagnitudeLocal = Magnitude.OfSubtype("Local");
		/// <summary>
		/// Describes the mantle seismic magnitude.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a number.</para>
		/// </remarks>
		public static readonly TagTypeKey MagnitudeMantle = Magnitude.OfSubtype("Mantle");
		/// <summary>
		/// Describes the moment seismic magnitude.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a number.</para>
		/// </remarks>
		public static readonly TagTypeKey MagnitudeMoment = Magnitude.OfSubtype("Moment");
		/// <summary>
		/// Describes the Richter seismic magnitude.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a number.</para>
		/// </remarks>
		public static readonly TagTypeKey MagnitudeRichter = Magnitude.OfSubtype("Richter");
		/// <summary>
		/// Describes the surface-wave seismic magnitude.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a number.</para>
		/// </remarks>
		public static readonly TagTypeKey MagnitudeSurfaceWave = Magnitude.OfSubtype("SurfaceWave");

		/// <summary>
		/// Describes whether the event is ongoing.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a boolean specifying whether the event is ongoing.</para>
		/// </remarks>
		public static readonly TagTypeKey Ongoing = "Ongoing";

		/// <summary>
		/// Describes the PGA (peak ground acceleration).
		/// </summary>
		/// <remarks>
		/// <para>The value should be a quantity. A number is also acceptable but not recommended, and when used, must be in gal (cm/s^2).</para>
		/// </remarks>
		public static readonly TagTypeKey PGA = "PGA";

		/// <summary>
		/// Describes the PGV (peak ground velocity).
		/// </summary>
		/// <remarks>
		/// <para>The value should be a quantity. A number is also acceptable but not recommended, and when used, must be in cm/s.</para>
		/// </remarks>
		public static readonly TagTypeKey PGV = "PGV";

		/// <summary>
		/// Describes the plume height above crater.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a quantity. A number is also acceptable but not recommended, and when used, must be in meters.</para>
		/// </remarks>
		public static readonly TagTypeKey PlumeHeightAboveCrater = "PlumeHeightAboveCrater";

		/// <summary>
		/// Describes the tsunami height.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a quantity. A number is also acceptable but not recommended, and when used, must be in meters.</para>
		/// </remarks>
		public static readonly TagTypeKey TsunamiHeight = "TsunamiHeight";

		/// <summary>
		/// Describes the time.
		/// </summary>
		/// <remarks>
		/// <para>This tag is a parent type. It may be used directly to describe when the event feature occurred. To describe another time, use a subtype.</para>
		/// <para>The value should be a date time offset.</para>
		/// </remarks>
		public static readonly TagTypeKey Time = "Time";
		/// <summary>
		/// Describes the time when the event feature was modified.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a date time offset.</para>
		/// </remarks>
		public static readonly TagTypeKey TimeModified = Time.OfSubtype("Modified");
		#endregion
	}
}
