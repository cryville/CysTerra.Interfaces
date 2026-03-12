namespace Cryville.EEW.Measure {
	/// <summary>
	/// The unit style.
	/// </summary>
	public enum UnitStyle {
		/// <summary>
		/// Use <see cref="NamedUnit.Symbol" />.
		/// </summary>
		Symbol = 0x00,
		/// <summary>
		/// Use <see cref="NamedUnit.ShortName" />.
		/// </summary>
		ShortName = 0x80,
		/// <summary>
		/// Use <see cref="NamedUnit.FullName" />.
		/// </summary>
		FullName = 0xff,
	}
}
