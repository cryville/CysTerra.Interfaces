using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Cryville.EEW.Analyzer {
	using static ResourceHelpers;
	using static Resources;

	[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
	public class UseSharedCultures : DiagnosticAnalyzer {
		static readonly LocalizableString Title = CreateLocalizableResourceString(nameof(UseSharedCulturesTitle));
		static readonly LocalizableString MessageFormat = CreateLocalizableResourceString(nameof(UseSharedCulturesMessageFormat));
		static readonly LocalizableString Description = CreateLocalizableResourceString(nameof(UseSharedCulturesDescription));

		public const string DiagnosticId = "CRYVEEW0001";
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
			context.RegisterOperationAction(ia.Analyze, OperationKind.Invocation, OperationKind.ObjectCreation, OperationKind.PropertyReference);
		}

		sealed class InternalAnalyzer {
			readonly INamedTypeSymbol CultureInfoType;
			readonly ImmutableHashSet<INamedTypeSymbol> TargetContainingTypes;
			readonly ImmutableHashSet<ISymbol> InvariantCultureProperties;
			public InternalAnalyzer(Compilation compilation) {
				CultureInfoType = compilation.GetType("System.Globalization.CultureInfo");
				TargetContainingTypes = [
					CultureInfoType,
					compilation.GetType("System.Threading.Thread"),
					compilation.GetType("System.Reflection.AssemblyName"),
				];
				InvariantCultureProperties = [
					CultureInfoType.GetSingleMember("InvariantCulture"),
					CultureInfoType.GetSingleMember("Parent"),
				];
			}

			public void Analyze(OperationAnalysisContext context) {
				if (context.Operation is IInvocationOperation invocationOp) {
					var method = invocationOp.TargetMethod;
					if (
						method.ReturnType.Equals(CultureInfoType, SymbolEqualityComparer.Default) &&
						TargetContainingTypes.Contains(method.ContainingType, SymbolEqualityComparer.Default)
					) {
						context.ReportDiagnostic(Diagnostic.Create(Rule, invocationOp.Syntax.GetLocation(), method.ToDisplayString(SymbolDisplayFormat.CSharpShortErrorMessageFormat)));
					}
				}
				else if (context.Operation is IObjectCreationOperation objCreationOp) {
					if (objCreationOp.Type is ITypeSymbol type && type.Equals(CultureInfoType, SymbolEqualityComparer.Default)) {
						context.ReportDiagnostic(Diagnostic.Create(Rule, objCreationOp.Syntax.GetLocation(), objCreationOp.Constructor?.ToDisplayString(SymbolDisplayFormat.CSharpShortErrorMessageFormat)));
					}
				}
				else if (context.Operation is IPropertyReferenceOperation propRefOp) {
					var prop = propRefOp.Property;
					if (
						propRefOp.Type is ITypeSymbol type && type.Equals(CultureInfoType, SymbolEqualityComparer.Default) &&
						TargetContainingTypes.Contains(prop.ContainingType, SymbolEqualityComparer.Default) &&
						!InvariantCultureProperties.Contains(prop, SymbolEqualityComparer.Default)
					) {
						context.ReportDiagnostic(Diagnostic.Create(Rule, propRefOp.Syntax.GetLocation(), prop.ToDisplayString(SymbolDisplayFormat.CSharpShortErrorMessageFormat)));
					}
				}
			}
		}
	}
}
