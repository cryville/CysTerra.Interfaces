using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Cryville.EEW.ComponentModel {
	/// <summary>
	/// Specifies a localizable display name.
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public sealed class LocalizableDisplayNameAttribute : DisplayNameAttribute, ILocalizableMetadataAttribute {
		/// <summary>
		/// The name of the localized resource.
		/// </summary>
		public string Type { get; }
		/// <summary>
		/// The string set names where the localizable string is.
		/// </summary>
		public string[] Path { get; set; } = [];

		string ILocalizableMetadataAttribute.Name => DisplayNameValue;

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
				var culture = SharedCultures.CurrentUICulture;
				return GetLocalizedValue(ref culture);
			}
		}
		/// <inheritdoc />
		public string GetLocalizedValue([NotNull] ref CultureInfo? culture) {
			using var lres = new LocalizedResource(Type, ref culture);
			var res = lres.RootMessageStringSet;
			foreach (var setName in Path) {
				res = res.GetStringSet(setName);
				if (res == null) {
					culture = CultureInfo.InvariantCulture;
					return DisplayNameValue;
				}
			}
			if (res.GetString(DisplayNameValue) is not string result) {
				culture = CultureInfo.InvariantCulture;
				return DisplayNameValue;
			}
			return result;
		}
	}
}
