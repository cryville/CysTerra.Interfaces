using System;
using System.Text;
using System.Xml;

namespace Cryville.EEW {
	/// <summary>
	/// Shared settings.
	/// </summary>
	public static class SharedSettings {
		/// <summary>
		/// A UTF-8 encoding without BOM.
		/// </summary>
		public static readonly Encoding Encoding = new UTF8Encoding(false, true);

		/// <summary>
		/// The shared user agent to be sent in Web requests.
		/// </summary>
		public static readonly string UserAgent = typeof(SharedSettings).Assembly.GetName().Version is Version ver ? $"CysTerra/{ver.ToString(3)}" : "CysTerra";

		/// <summary>
		/// The shared XML reader settings.
		/// </summary>
		public static readonly XmlReaderSettings XmlReaderSettings = new() {
			DtdProcessing = DtdProcessing.Ignore,
		};
	}
}
