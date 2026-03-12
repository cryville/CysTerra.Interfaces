using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace Cryville.EEW.Analyzer {
	static class AnalyzerHelpers {
		public static INamedTypeSymbol GetType(this Compilation compilation, string name) => compilation.GetTypeByMetadataName(name)
			?? throw new TypeLoadException($"The type {name} is not found.");
		public static ISymbol GetSingleMember(this ITypeSymbol type, string name) => type.GetMembers(name).Single();
		public static IMethodSymbol GetMethod(this ITypeSymbol type, string name, params ITypeSymbol[] types) => type.GetMembers(name).OfType<IMethodSymbol>().Single(m => m.Parameters.Select(p => p.Type).SequenceEqual(types, SymbolEqualityComparer.Default));
	}
}
