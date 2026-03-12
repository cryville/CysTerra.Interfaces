namespace Cryville.EEW {
	/// <summary>
	/// Represents the method that will handle an event when the event provides data.
	/// </summary>
	/// <typeparam name="T">The type of the event data.</typeparam>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	public delegate void Handler<in T>(object sender, T e);
}
