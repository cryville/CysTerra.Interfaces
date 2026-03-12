using System;
using System.ComponentModel;

namespace Cryville.EEW.ComponentModel {
	/// <summary>
	/// Specifies a localizable description.
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public sealed class LocalizableDescriptionAttribute : DescriptionAttribute {
		/// <summary>
		/// The name of the localized resource.
		/// </summary>
		public string Type { get; }
		/// <summary>
		/// The string set names where the localizable string is.
		/// </summary>
		public string[] Path { get; set; } = [];

		/// <summary>
		/// Creates an instance of the <see cref="LocalizableDescriptionAttribute" /> class.
		/// </summary>
		/// <param name="description">The name of the localizable string.</param>
		/// <param name="type">The name of the localizable resource.</param>
		public LocalizableDescriptionAttribute(string description, string type = "") : base(description) {
			Type = type;
		}

		/// <inheritdoc />
		public override string Description {
			get {
				using var lres = new LocalizedResource(Type, SharedCultures.CurrentUICulture);
				var res = lres.RootMessageStringSet;
				foreach (var setName in Path) {
					res = res.GetStringSet(setName);
					if (res == null) {
						return DescriptionValue;
					}
				}
				return res.GetString(DescriptionValue) ?? DescriptionValue;
			}
		}
	}
}
