using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Cryville.EEW {
	/// <summary>
	/// Represents access to a localized resource set.
	/// </summary>
	[SuppressMessage("CodeQuality", "IDE0079", Justification = "False report")]
	[SuppressMessage("Performance", "CA1815", Justification = "Not equatable")]
	public readonly struct LocalizedResource : IDisposable {
		/// <summary>
		/// The root message string set of the resource.
		/// </summary>
		public IMessageStringSet RootMessageStringSet { get; private init; }

		/// <summary>
		/// Creates a new instance of the <see cref="LocalizedResource" /> struct.
		/// </summary>
		/// <param name="type">The name of the localized resource.</param>
		/// <param name="culture">The preferred culture of the resource.</param>
		/// <exception cref="InvalidOperationException">The resources have not been initialized yet.</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public LocalizedResource(string type, CultureInfo? culture) {
			if (LocalizedResources.Manager is not ILocalizedResourceManager manager)
				throw new InvalidOperationException("Resources not initialized.");
			RootMessageStringSet = manager.GetRootStringSet(LocalizedResources.GetAssemblyName(Assembly.GetCallingAssembly()), type, ref culture);
		}
		/// <summary>
		/// Creates a new instance of the <see cref="LocalizedResource" /> struct.
		/// </summary>
		/// <param name="type">The name of the localized resource.</param>
		/// <param name="culture">The preferred culture of the resource. When the constructor returns, set to the actual culture of the resource.</param>
		/// <exception cref="InvalidOperationException">The resources have not been initialized yet.</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public LocalizedResource(string type, [NotNull] ref CultureInfo? culture) {
			if (LocalizedResources.Manager is not ILocalizedResourceManager manager)
				throw new InvalidOperationException("Resources not initialized.");
			RootMessageStringSet = manager.GetRootStringSet(LocalizedResources.GetAssemblyName(Assembly.GetCallingAssembly()), type, ref culture);
		}
		/// <inheritdoc />
		public void Dispose() { }
	}

	/// <summary>
	/// Represents access to a string set in a localized resource.
	/// </summary>
	public interface IMessageStringSet {
		/// <summary>
		/// Gets a string in the string set.
		/// </summary>
		/// <param name="name">The name of the string.</param>
		/// <returns>The string of the specified name in the string set, or <see langword="null" /> if not found.</returns>
		string? GetString(string name);
		/// <summary>
		/// Gets a string set in the string set.
		/// </summary>
		/// <param name="name">The name of the string set.</param>
		/// <returns>The string set of the specified name in the string set, or <see langword="null" /> if not found.</returns>
		IMessageStringSet? GetStringSet(string name);
	}
}
