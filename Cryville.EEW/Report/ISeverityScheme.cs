using System.Collections.Generic;
using System.Linq;
using Tag = System.Collections.Generic.KeyValuePair<Cryville.EEW.TagTypeKey, object?>;

namespace Cryville.EEW.Report {
	/// <summary>
	/// Represents a severity scheme, extracting severity values from different properties.
	/// </summary>
	/// <remarks>
	/// Severity values are defined based on the human perception of a specific property. When a property is not directly linked to human perception, a relative value based on mean conditions is used.
	/// <list type="table">
	/// <item><term><c>0.00</c></term><description>Not perceivable by human and only detected by instruments; very light</description></item>
	/// <item><term><c>0.50</c></term><description>Slightly perceived by human; light</description></item>
	/// <item><term><c>0.75</c></term><description>Perceived by human; moderate</description></item>
	/// <item><term><c>1.00</c></term><description>Strongly perceived by human; heavy</description></item>
	/// <item><term><c>1.25</c></term><description>Violently perceived by human; disastrous; extreme</description></item>
	/// </list>
	/// </remarks>
	public interface ISeverityScheme {
		/// <summary>
		/// Extracts a severity value from a property.
		/// </summary>
		/// <param name="type">The type of the property.</param>
		/// <param name="value">The value of the property.</param>
		/// <returns>The severity value.</returns>
		float From(TagTypeKey type, object? value);
		/// <summary>
		/// Extracts a severity value from a set properties.
		/// </summary>
		/// <param name="props">The properties.</param>
		/// <returns>The severity value.</returns>
		float From(IEnumerable<Tag> props) {
			var prop = props.FirstOrDefault();
			return From(prop.Key, prop.Value);
		}
	}
}
