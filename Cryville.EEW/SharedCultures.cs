using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Cryville.EEW {
	/// <summary>
	/// Shared cultures.
	/// </summary>
	public static class SharedCultures {
		static readonly Dictionary<string, WrappedCultureInfo> _map = [];

		[ThreadStatic]
		static CultureInfo? m_currentCulture;
		/// <summary>
		/// The current culture.
		/// </summary>
		[SuppressMessage("CodeQuality", "IDE0079", Justification = "False report")]
		[SuppressMessage("Globalization", "CRYVEEW0001")]
		public static CultureInfo CurrentCulture {
			get => m_currentCulture ??= Get(CultureInfo.CurrentCulture.Name);
			internal set => CultureInfo.CurrentCulture = m_currentCulture = value;
		}

		[ThreadStatic]
		static CultureInfo? m_currentUICulture;
		/// <summary>
		/// The current UI culture.
		/// </summary>
		[SuppressMessage("CodeQuality", "IDE0079", Justification = "False report")]
		[SuppressMessage("Globalization", "CRYVEEW0001")]
		public static CultureInfo CurrentUICulture {
			get => m_currentUICulture ??= Get(CultureInfo.CurrentUICulture.Name);
			internal set => CultureInfo.CurrentUICulture = m_currentUICulture = value;
		}

		/// <summary>
		/// Gets a culture of the specified name.
		/// </summary>
		/// <param name="name">The name of the culture.</param>
		/// <returns>The culture of the specified name.</returns>
		public static CultureInfo Get(string name) {
			if (name == "und")
				return CultureInfo.InvariantCulture;
			return InternalGet(name);
		}
		static WrappedCultureInfo InternalGet(string name) {
			lock (_map) {
				if (!_map.TryGetValue(name, out var result)) {
					_map.Add(name, result = new(name));
				}
				return result;
			}
		}
		sealed class WrappedCultureInfo : CultureInfo {
			public WrappedCultureInfo(string name) : base(name) { }

			DateTimeFormatInfo? m_dateTimeFormatInfo;
			public override DateTimeFormatInfo DateTimeFormat {
				get {
					if (m_dateTimeFormatInfo == null) {
						m_dateTimeFormatInfo = base.DateTimeFormat;
						m_dateTimeFormatInfo.ShortTimePattern = FixTimePattern(m_dateTimeFormatInfo.ShortTimePattern);
						m_dateTimeFormatInfo.LongTimePattern = FixTimePattern(m_dateTimeFormatInfo.LongTimePattern);
						m_dateTimeFormatInfo.FullDateTimePattern = FixTimePattern(m_dateTimeFormatInfo.FullDateTimePattern);
					}
					return m_dateTimeFormatInfo;
				}
				set => base.DateTimeFormat = value;
			}

			WrappedCultureInfo? m_parent;
			public override CultureInfo Parent => m_parent ??= InternalGet(base.Parent.Name);
		}
		static string FixTimePattern(string pattern) {
			bool use12Hour = false;
			bool useAMPM = false;
			for (var i = 0; i < pattern.Length; i++) {
				switch (pattern[i]) {
					case 'h': use12Hour = true; break;
					case 't': useAMPM = true; break;
					case '\\': i++; break;
					case '\'':
						for (i++; i < pattern.Length; i++) {
							var c = pattern[i];
							if (c == '\'') break;
							if (c == '\\') i++;
						}
						break;
				}
			}
			if (!use12Hour || useAMPM) return pattern;
			char[] result = pattern.ToCharArray();
			for (var i = 0; i < result.Length; i++) {
				switch (result[i]) {
					case 'h': result[i] = 'H'; break;
					case '\\': i++; break;
					case '\'':
						for (i++; i < result.Length; i++) {
							var c = result[i];
							if (c == '\'') break;
							if (c == '\\') i++;
						}
						break;
				}
			}
			return new string(result);
		}
	}
}
