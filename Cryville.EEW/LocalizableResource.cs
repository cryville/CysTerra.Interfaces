using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Cryville.EEW {
	/// <summary>
	/// Represents access to a localizable resource set.
	/// </summary>
	[SuppressMessage("CodeQuality", "IDE0079", Justification = "False report")]
	[SuppressMessage("Performance", "CA1815", Justification = "Not equatable")]
	public readonly struct LocalizableResource : IDisposable {
		/// <summary>
		/// The root message string set of the resource.
		/// </summary>
		public ILocalizableMessageStringSet RootMessageStringSet { get; private init; }

		/// <summary>
		/// Creates a new instance of the <see cref="LocalizedResource" /> struct.
		/// </summary>
		/// <param name="type">The name of the localized resource.</param>
		/// <exception cref="InvalidOperationException">The resources have not been initialized yet.</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public LocalizableResource(string type) {
			if (LocalizedResources.Manager is not ILocalizedResourceManager manager)
				throw new InvalidOperationException("Resources not initialized.");
			RootMessageStringSet = manager.GetLocalizableRootStringSet(LocalizedResources.GetAssemblyName(Assembly.GetCallingAssembly()), type);
		}
		/// <inheritdoc />
		public void Dispose() { }
	}

	/// <summary>
	/// Represents access to a string set in a localizable resource.
	/// </summary>
	public interface ILocalizableMessageStringSet {
		/// <summary>
		/// Gets a string in the string set.
		/// </summary>
		/// <param name="name">The name of the string.</param>
		/// <returns>The string of the specified name in the string set, or <see langword="null" /> if not found.</returns>
		ILocalizable<string?> GetString(string name);
		/// <summary>
		/// Gets a string set in the string set.
		/// </summary>
		/// <param name="name">The name of the string set.</param>
		/// <returns>The string set of the specified name in the string set, or <see langword="null" /> if not found.</returns>
		ILocalizableMessageStringSet GetStringSet(string name);
	}
}
