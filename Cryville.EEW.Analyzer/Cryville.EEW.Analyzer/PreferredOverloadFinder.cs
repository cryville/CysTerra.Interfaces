using Microsoft.CodeAnalysis;
using System.Collections.Concurrent;
using System.Linq;

namespace Cryville.EEW.Analyzer {
	sealed class PreferredOverloadFinder {
		readonly INamedTypeSymbol TargetParameterType;
		readonly INamedTypeSymbol ObsoleteAttributeType;
		readonly ConcurrentDictionary<ISymbol, ISymbol?> _map = new(SymbolEqualityComparer.Default);

		public PreferredOverloadFinder(Compilation compilation, INamedTypeSymbol targetParameterType) {
			TargetParameterType = targetParameterType;
			ObsoleteAttributeType = compilation.GetType("System.ObsoleteAttribute");
		}

		public ISymbol? FindPreferredOverload(IMethodSymbol method) {
			if (!_map.TryGetValue(method, out var result))
				_map.TryAdd(method, result = Analyze(method));
			return result;
		}
		public void AddException(IMethodSymbol originalMethod, IMethodSymbol? preferredMethod) {
			_map.TryAdd(originalMethod, preferredMethod);
		}
		ISymbol? Analyze(IMethodSymbol method) {
			if (method.Parameters.Any(p => p.Type.Equals(TargetParameterType, SymbolEqualityComparer.Default)))
				return null;
			var methods = method.ContainingType.GetMembers(method.Name)
				.OfType<IMethodSymbol>()
				.Where(m =>
					m.Parameters.Any(p => p.Type.Equals(TargetParameterType, SymbolEqualityComparer.Default)) &&
					!m.GetAttributes().Any(a => a.AttributeClass is INamedTypeSymbol sym && sym.Equals(ObsoleteAttributeType, SymbolEqualityComparer.Default))
				)
				.ToArray();
			if (methods.Length == 0)
				return null;
			return methods.Where(m => IsCandidate(method, m)).OrderBy(m => m.Parameters.Length).FirstOrDefault();
		}
		static bool IsCandidate(IMethodSymbol self, IMethodSymbol target) {
			if (SymbolEqualityComparer.Default.Equals(self, target)) return false;
			var selfParams = self.Parameters;
			var targetParams = target.Parameters;
			if (targetParams.Length <= selfParams.Length) return false;
			int targetIndex = 0;
			for (int selfIndex = 0; selfIndex < selfParams.Length; selfIndex++) {
				var selfParamType = selfParams[selfIndex].Type;
				while (true) {
					if (targetIndex >= targetParams.Length) return false;
					var targetParamType = targetParams[targetIndex++].Type;
					if (SymbolEqualityComparer.Default.Equals(selfParamType, targetParamType)) break;
				}
			}
			return true;
		}
	}
}
