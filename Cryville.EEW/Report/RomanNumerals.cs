using Cryville.Common.Compat;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Cryville.EEW.Report {
	/// <summary>
	/// Provides a set of <see langword="static" /> methods related to Roman numerals.
	/// </summary>
	[Experimental("CRYVEEW5001")]
	public static class RomanNumerals {
		/// <summary>
		/// Creates an instance of the <see cref="ReportProperty" /> class with a value represented with a Roman numeral character.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="key">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="culture">The culture.</param>
		/// <param name="severityScheme">The severity scheme.</param>
		/// <param name="accuracyOrder">The accuracy of the value (<see cref="ReportProperty.AccuracyOrder" />).</param>
		/// <returns>An instance of the <see cref="ReportProperty" /> class.</returns>
		public static ReportProperty CreateRomanIntensityProperty(TagTypeKey type, string? key, float value, CultureInfo culture, ISeverityScheme severityScheme, int accuracyOrder) => new(type, key, ToRomanNumeralChar(Math.Clamp((int)MathF.Round(value), 1, 12), culture), severityScheme, value) { AccuracyOrder = accuracyOrder, Condition = string.Format(culture, "({0})", value.ToString("F1", culture)) };

		/// <summary>
		/// Converts an integer to the corresponding Roman numeral character.
		/// </summary>
		/// <param name="num">The number.</param>
		/// <param name="culture">The culture.</param>
		/// <returns>The uppercase Roman numeral character corresponding to <paramref name="num" />, or a generic string representation of <paramref name="num" /> if it is not between 1 and 12 (both inclusive).</returns>
		public static string ToRomanNumeralChar(int num, CultureInfo culture) => num is >= 1 and <= 12 ? ((char)('\x2160' + num - 1)).ToString() : num.ToString(culture);

		static readonly Dictionary<char, int> _romanMap = new() {
			{ 'I', 1 },
			{ 'V', 5 },
			{ 'X', 10 },
			{ 'L', 50 },
			{ 'C', 100 },
			{ 'D', 500 },
			{ 'M', 1000 },
		};
		/// <summary>
		/// Converts a multi-character Roman numeral to an integer.
		/// </summary>
		/// <param name="roman">The multi-character Roman numeral.</param>
		/// <returns>The integer corresponding to <paramref name="roman" />.</returns>
		public static int RomanToInteger(string roman) {
			ThrowHelper.ThrowIfNull(roman);
			int number = 0;
			for (int i = 0; i < roman.Length; i++) {
				int num = _romanMap[roman[i]];
				int i1 = i + 1;
				if (i1 < roman.Length && num < _romanMap[roman[i1]])
					number -= num;
				else
					number += num;
			}
			return number;
		}

		static readonly (int, string)[] _numRomanList = [
			(1000, "M"),
			(900, "CM"),
			(500, "D"),
			(400, "CD"),
			(100, "C"),
			(90, "XC"),
			(50, "L"),
			(40, "XL"),
			(10, "X"),
			(9, "IX"),
			(5, "V"),
			(4, "IV"),
			(1, "I"),
		];
		/// <summary>
		/// Converts an integer to a multi-character Roman numeral.
		/// </summary>
		/// <param name="number">The integer.</param>
		/// <returns>The multi-character Roman numeral correspoding to <paramref name="number" />.</returns>
		public static string IntegerToRoman(int number) {
			var sb = new StringBuilder();
			foreach (var item in _numRomanList) {
				int num = item.Item1;
				while (number >= num) {
					sb.Append(item.Item2);
					number -= num;
				}
			}
			return sb.ToString();
		}
	}
}
