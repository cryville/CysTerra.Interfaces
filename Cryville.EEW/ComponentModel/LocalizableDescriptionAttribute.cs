using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Cryville.EEW.ComponentModel {
	/// <summary>
	/// Specifies a localizable description.
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public sealed class LocalizableDescriptionAttribute : DescriptionAttribute, ILocalizableMetadataAttribute {
		/// <summary>
		/// The name of the localized resource.
		/// </summary>
		public string Type { get; }
		/// <summary>
		/// The string set names where the localizable string is.
		/// </summary>
		public string[] Path { get; set; } = [];

		string ILocalizableMetadataAttribute.Name => DescriptionValue;

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
					return DescriptionValue;
				}
			}
			if (res.GetString(DescriptionValue) is not string result) {
				culture = CultureInfo.InvariantCulture;
				return DescriptionValue;
			}
			return result;
		}
	}
}
