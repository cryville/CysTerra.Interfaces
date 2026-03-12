namespace Cryville.EEW.Report {
	/// <summary>
	/// An empty <see cref="ISeverityScheme" /> that always returns <c>-1</c>.
	/// </summary>
	public class EmptySeverityScheme : ISeverityScheme {
		static EmptySeverityScheme? s_instance;
		/// <summary>
		/// The shared instance of the <see cref="EmptySeverityScheme" /> class.
		/// </summary>
		public static EmptySeverityScheme Instance => s_instance ??= new();

		/// <inheritdoc />
		public float From(TagTypeKey type, object? value) => -1;
	}
}
