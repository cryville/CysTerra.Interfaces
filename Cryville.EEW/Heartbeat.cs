namespace Cryville.EEW {
	/// <summary>
	/// Represents a heartbeat event.
	/// </summary>
	public record struct Heartbeat {
		/// <summary>
		/// Gets an empty instance of the <see cref="Heartbeat" /> struct.
		/// </summary>
		public static Heartbeat Instance => new();
	}
}
