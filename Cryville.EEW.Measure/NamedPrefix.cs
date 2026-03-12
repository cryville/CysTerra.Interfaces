namespace Cryville.EEW.Measure {
	/// <summary>
	/// Named unit prefix.
	/// </summary>
	/// <param name="Scale">The scale.</param>
	/// <param name="SymbolAffix">The culture-independent symbol affix template.</param>
	/// <param name="ShortNameAffix">The short name affix template.</param>
	/// <param name="FullNameAffix">The full name affix template.</param>
	public readonly record struct NamedPrefix(double Scale, string SymbolAffix, ILocalizable<string> ShortNameAffix, ILocalizable<string> FullNameAffix) {
		/// <summary>
		/// Creates an instance of the <see cref="NamedPrefix" /> struct.
		/// </summary>
		/// <param name="scale">The scale.</param>
		/// <param name="symbolAffix">The culture-independent symbol affix template.</param>
		/// <param name="nameAffix">The name affix template.</param>
		public NamedPrefix(double scale, string symbolAffix, ILocalizable<string> nameAffix) : this(scale, symbolAffix, nameAffix, nameAffix) { }
	}
}
