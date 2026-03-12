namespace Cryville.EEW {
	/// <summary>
	/// Provides a set of special <see cref="TagTypeKey" />.
	/// </summary>
	public static class SpecialTagTypeKeys {
		/// <summary>
		/// The copula tag type key.
		/// </summary>
		/// <remarks>
		/// The value should be a <see cref="TagTypeKey" /> defining the nature and the general category of the feature.
		/// </remarks>
		public static readonly TagTypeKey Is = "";

		/// <summary>
		/// The inclusion tag type key.
		/// </summary>
		/// <remarks>
		/// The value should be an enumerable of the features that are included in the current feature.
		/// </remarks>
		public static readonly TagTypeKey Includes = ":";

		/// <summary>
		/// The location tag type key.
		/// </summary>
		/// <remarks>
		/// The value should be a feature indicating the location where the event occurred.
		/// </remarks>
		public static readonly TagTypeKey At = "@";

		/// <summary>
		/// Describes the severity of the event feature.
		/// </summary>
		/// <remarks>
		/// <para>The value should be a floating-point number. See <see cref="Report.ISeverityScheme" />.</para>
		/// </remarks>
		public static readonly TagTypeKey Severity = "Severity";
	}
}
