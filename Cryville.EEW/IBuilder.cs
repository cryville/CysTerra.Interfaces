using Cryville.EEW.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Cryville.EEW {
	/// <summary>
	/// Represents a builder that builds another object. May hold a set of properties used when constructing the target object.
	/// </summary>
	public interface IBuilder : IPropertiesHolder, INamedComponent {
		/// <summary>
		/// Builds the object.
		/// </summary>
		/// <param name="culture">The preferred culture of the object. When the method returns, set to the actual culture of the object.</param>
		/// <returns>The built object.</returns>
		object? Build(ref CultureInfo? culture);
	}
	/// <summary>
	/// Represents a builder that builds another object. May hold a set of properties used when constructing the target object.
	/// </summary>
	/// <typeparam name="T">The type of the target object.</typeparam>
	public interface IBuilder<out T> : IBuilder {
		object? IBuilder.Build(ref CultureInfo? culture) => Build(ref culture);
		/// <summary>
		/// Builds the object.
		/// </summary>
		/// <param name="culture">The preferred culture of the object. When the method returns, set to the actual culture of the object.</param>
		/// <returns>The built object.</returns>
		new T Build(ref CultureInfo? culture);
	}
	/// <summary>
	/// Represents a builder that builds another object with a parameterless constructor.
	/// </summary>
	/// <typeparam name="T">The type of the target object.</typeparam>
	public abstract class SimpleBuilder<T> : IBuilder<T> where T : new() {
		/// <inheritdoc />
		public abstract string? GetName([NotNull] ref CultureInfo? culture);
		/// <inheritdoc />
		public T Build(ref CultureInfo? culture) => new();
	}
}
