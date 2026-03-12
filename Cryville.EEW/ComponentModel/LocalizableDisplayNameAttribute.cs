using System;
using System.ComponentModel;

namespace Cryville.EEW.ComponentModel {
	/// <summary>
	/// Specifies a localizable display name.
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public sealed class LocalizableDisplayNameAttribute : DisplayNameAttribute {
		/// <summary>
		/// The name of the localized resource.
		/// </summary>
		public string Type { get; }
		/// <summary>
		/// The string set names where the localizable string is.
		/// </summary>
		public string[] Path { get; set; } = [];

		/// <summary>
		/// Creates an instance of the <see cref="LocalizableDisplayNameAttribute" /> class.
		/// </summary>
		/// <param name="displayName">The name of the localizable string.</param>
		/// <param name="type">The name of the localizable resource.</param>
		public LocalizableDisplayNameAttribute(string displayName, string type = "") : base(displayName) {
			Type = type;
		}

		/// <inheritdoc />
		public override string DisplayName {
			get {
				using var lres = new LocalizedResource(Type, SharedCultures.CurrentUICulture);
				var res = lres.RootMessageStringSet;
				foreach (var setName in Path) {
					res = res.GetStringSet(setName);
					if (res == null) {
						return DisplayNameValue;
					}
				}
				return res.GetString(DisplayNameValue) ?? DisplayNameValue;
			}
		}
	}
}
