using System;
using System.Globalization;

namespace Cryville.EEW {
	/// <summary>
	/// Represents a generator that generates objects of a specific type from input objects.
	/// </summary>
	/// <typeparam name="TOut">The type of the generated objects.</typeparam>
	public interface IGenerator<out TOut> {
		/// <summary>
		/// Generates the object.
		/// </summary>
		/// <param name="e">The input object.</param>
		/// <param name="culture">The preferred culture of the generated object. When the method returns, set to the actual culture of the generated object.</param>
		/// <returns>The generated object.</returns>
		TOut Generate(object e, ref CultureInfo culture);
	}
	/// <summary>
	/// Represents a generator that generates objects of a specific type from input objects of another specific type.
	/// </summary>
	/// <typeparam name="TIn">The type of the input objects.</typeparam>
	/// <typeparam name="TOut">The type of the generated objects.</typeparam>
	public interface IGenerator<in TIn, out TOut> : IGenerator<TOut> {
		TOut IGenerator<TOut>.Generate(object e, ref CultureInfo culture) {
			if (e is not TIn te)
				throw new InvalidCastException("Invalid input type.");
			return Generate(te, ref culture);
		}
		/// <summary>
		/// Generates the object.
		/// </summary>
		/// <param name="e">The input object.</param>
		/// <param name="culture">The preferred culture of the generated object. When the method returns, set to the actual culture of the generated object.</param>
		/// <returns>The generated object.</returns>
		TOut Generate(TIn e, ref CultureInfo culture);
	}
	/// <summary>
	/// Represents a generator that generates objects of a specific type from input objects, with contexts.
	/// </summary>
	/// <typeparam name="TContext">The type of the context.</typeparam>
	/// <typeparam name="TOut">The type of the generated objects.</typeparam>
	public interface IContextedGenerator<in TContext, out TOut> : IGenerator<TOut> where TContext : class {
		TOut IGenerator<TOut>.Generate(object e, ref CultureInfo culture) {
			return Generate(e, null, ref culture);
		}
		/// <summary>
		/// Generates the object.
		/// </summary>
		/// <param name="e">The input object.</param>
		/// <param name="context">The context.</param>
		/// <param name="culture">The preferred culture of the generated object. When the method returns, set to the actual culture of the generated object.</param>
		/// <returns>The generated object.</returns>
		TOut Generate(object e, TContext? context, ref CultureInfo culture);
	}
	/// <summary>
	/// Represents a generator that generates objects of a specific type from input objects of another specific type, with contexts.
	/// </summary>
	/// <typeparam name="TIn">The type of the input objects.</typeparam>
	/// <typeparam name="TContext">The type of the context.</typeparam>
	/// <typeparam name="TOut">The type of the generated objects.</typeparam>
	public interface IContextedGenerator<in TIn, in TContext, out TOut> : IGenerator<TIn, TOut>, IContextedGenerator<TContext, TOut> where TContext : class {
		TOut IGenerator<TOut>.Generate(object e, ref CultureInfo culture) {
			return Generate(e, null, ref culture);
		}
		TOut IGenerator<TIn, TOut>.Generate(TIn e, ref CultureInfo culture) {
			return Generate(e, null, ref culture);
		}
		TOut IContextedGenerator<TContext, TOut>.Generate(object e, TContext? context, ref CultureInfo culture) {
			if (e is not TIn te)
				throw new InvalidCastException("Invalid input type.");
			return Generate(te, context, ref culture);
		}
		/// <summary>
		/// Generates the object.
		/// </summary>
		/// <param name="e">The input object.</param>
		/// <param name="context">The context.</param>
		/// <param name="culture">The preferred culture of the generated object. When the method returns, set to the actual culture of the generated object.</param>
		/// <returns>The generated object.</returns>
		TOut Generate(TIn e, TContext? context, ref CultureInfo culture);
	}
}
