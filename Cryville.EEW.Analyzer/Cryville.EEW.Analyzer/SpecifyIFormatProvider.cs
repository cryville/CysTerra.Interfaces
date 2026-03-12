using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;

namespace Cryville.EEW.Analyzer {
	using static ResourceHelpers;
	using static Resources;

	[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
	public class SpecifyIFormatProvider : DiagnosticAnalyzer {
		static readonly LocalizableString Title = CreateLocalizableResourceString(nameof(SpecifyIFormatProviderTitle));
		static readonly LocalizableString MessageFormat = CreateLocalizableResourceString(nameof(SpecifyIFormatProviderMessageFormat));
		static readonly LocalizableString Description = CreateLocalizableResourceString(nameof(SpecifyIFormatProviderDescription));

		public const string DiagnosticId = "CRYVEEW0002";
		const string Category = "Globalization";
		static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true, Description);
		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

		public override void Initialize(AnalysisContext context) {
			if (context is null) throw new ArgumentNullException(nameof(context));
			context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
			context.EnableConcurrentExecution();

			context.RegisterCompilationStartAction(InitializeWorker);
		}

		void InitializeWorker(CompilationStartAnalysisContext context) {
			var ia = new InternalAnalyzer(context.Compilation);
			context.RegisterOperationAction(ia.Analyze, OperationKind.Invocation);
		}

		sealed class InternalAnalyzer {
			readonly INamedTypeSymbol IFormatProviderType;
			readonly INamedTypeSymbol ObsoleteAttributeType;
			readonly ConcurrentDictionary<ISymbol, ISymbol?> _map = new(SymbolEqualityComparer.Default);

			public InternalAnalyzer(Compilation compilation) {
				IFormatProviderType = compilation.GetType("System.IFormatProvider");
				ObsoleteAttributeType = compilation.GetType("System.ObsoleteAttribute");

				// Add excluded methods
				_map.TryAdd(compilation.GetType("System.Char").GetMethod("ToString"), null);
				_map.TryAdd(compilation.GetType("System.Text.StringBuilder").GetMethod("AppendLine"), null);
			}

			public void Analyze(OperationAnalysisContext context) {
				if (context.Operation is not IInvocationOperation invocationOp)
					return;
				var method = invocationOp.TargetMethod;
				if (!_map.TryGetValue(method, out var result))
					_map.TryAdd(method, result = Analyze(method));
				if (result == null)
					return;
				context.ReportDiagnostic(Diagnostic.Create(Rule, invocationOp.Syntax.GetLocation(), method.ToDisplayString(SymbolDisplayFormat.CSharpShortErrorMessageFormat), result.ToDisplayString(SymbolDisplayFormat.CSharpShortErrorMessageFormat)));
			}
			ISymbol? Analyze(IMethodSymbol method) {
				if (method.Parameters.Any(p => p.Type.Equals(IFormatProviderType, SymbolEqualityComparer.Default)))
					return null;
				var methods = method.ContainingType.GetMembers(method.Name)
					.OfType<IMethodSymbol>()
					.Where(m =>
						m.Parameters.Any(p => p.Type.Equals(IFormatProviderType, SymbolEqualityComparer.Default)) &&
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
}
