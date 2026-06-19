using Cryville.EEW.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Cryville.EEW.Extensions {
	public class ExtensionResolver {
		public LocalizedResource? ResolverMessageResource { get; set; }
		string GetResolverMessage(string key) => ResolverMessageResource?.RootMessageStringSet.GetString(key) ?? key;

		List<string>? _cachedAssemblyPaths;
		protected virtual List<string> GetAssemblyPaths() {
			if (_cachedAssemblyPaths != null)
				return _cachedAssemblyPaths;
			var assemblyPaths = new List<string>();
			assemblyPaths.AddRange(Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll"));
			if (Assembly.GetEntryAssembly() is Assembly entryAssembly && Path.GetDirectoryName(entryAssembly.Location) is string entryAssemblyDir) {
				assemblyPaths.AddRange(Directory.GetFiles(entryAssemblyDir, "*.dll"));
			}
			return _cachedAssemblyPaths = assemblyPaths;
		}

		T? GetAttrbute<T>(IEnumerable<CustomAttributeData> attrData, MetadataLoadContext context) where T : Attribute {
			var baseType = MapRuntimeToMetadataType(typeof(T), context);
			if (baseType == null)
				return null;
			var attr = attrData.SingleOrDefault(attr => {
				try {
					return baseType.IsAssignableFrom(attr.AttributeType);
				}
				catch (FileNotFoundException) {
					return false;
				}
			});
			if (attr == null)
				return null;
			var type = MapMetadataToRuntimeType(attr.AttributeType);
			Type[] ctorParams = [.. attr.Constructor.GetParameters().Select(i => MapMetadataToRuntimeType(i.ParameterType))];
			var ctor = type.GetConstructor(ctorParams) ?? throw new TypeLoadException(GetResolverMessage("ExtensionErrorUnsupportedAttribute"));
			object?[] args = [.. attr.ConstructorArguments.Select(arg => MapMetadataToRuntimeValue(arg.Value))];
			var result = (T)ctor.Invoke(args);
			foreach (var arg in attr.NamedArguments) {
				string name = arg.MemberName;
				var propertyInfo = type.GetProperty(name) ?? throw new TypeLoadException(GetResolverMessage("ExtensionErrorUnsupportedAttribute"));
				propertyInfo.SetValue(result, MapMetadataToRuntimeValue(arg.TypedValue.Value));
			}
			return result;
		}
		Type MapMetadataToRuntimeType(Type type) {
			if (type.AssemblyQualifiedName is not string name || Type.GetType(name) is not Type result) {
				throw new TypeLoadException(GetResolverMessage("ExtensionErrorUnsupportedAttribute"));
			}
			return result;
		}
		object? MapMetadataToRuntimeValue(object? obj) {
			if (obj is Type type)
				return MapMetadataToRuntimeType(type);
			if (obj is Type[] types)
				return types.Select(MapMetadataToRuntimeType).ToArray();
			return obj;
		}
		static Type? MapRuntimeToMetadataType(Type type, MetadataLoadContext context) {
			var assembly = context.LoadFromAssemblyPath(type.Assembly.Location);
			if (type.FullName is not string fullName)
				return null;
			var result = assembly.GetType(fullName);
			return result;
		}
		public async Task<ExtensionInfo> ResolveDllAsync(string dllFile, DirectoryInfo dirInfo, string expectedAssemblyName, CancellationToken cancellationToken) {
			var resolver = new PathAssemblyResolver([dllFile, .. GetAssemblyPaths()]);
			using var context = new MetadataLoadContext(resolver, typeof(object).Assembly.FullName);
			using var fileStream = new FileStream(dllFile, FileMode.Open, FileAccess.Read, FileShare.Read);
			using var bufferStream = new MemoryStream();
			await fileStream.CopyToAsync(bufferStream, cancellationToken).ConfigureAwait(true);
			bufferStream.Position = 0;
			using var sha512 = SHA512.Create();
			var hash = sha512.ComputeHash(bufferStream);
			bufferStream.Position = 0;
			var assembly = context.LoadFromStream(bufferStream);
			var assemblyName = assembly.GetName();
			string? name = assemblyName.Name;
			if (name != expectedAssemblyName)
				throw new InvalidOperationException(GetResolverMessage("ExtensionErrorNameMismatch"));
			var version = assemblyName.Version;
			using var resScope = Localization.EnterScope(new TemporaryLocalizedResourceManager(dirInfo.FullName));
			var attrData = assembly.GetCustomAttributesData();
			string displayName = GetAttrbute<DisplayNameAttribute>(attrData, context)?.DisplayName ?? GetAttrbute<AssemblyTitleAttribute>(attrData, context)?.Title ?? name;
			var displayNameCulture = resScope.ReadLastReturnedCulture();
			string? description = GetAttrbute<DescriptionAttribute>(attrData, context)?.Description ?? GetAttrbute<AssemblyDescriptionAttribute>(attrData, context)?.Description;
			var descriptionCulture = resScope.ReadLastReturnedCulture();
			return new(name, displayName, displayNameCulture, description, descriptionCulture, version, hash, assembly.GetReferencedAssemblies());
		}
	}
}
