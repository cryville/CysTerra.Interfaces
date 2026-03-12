using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Cryville.EEW.Analyzer {
	using static ResourceHelpers;
	using static Resources;

	[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
	public class SelfReferencingGenericStructStaticField : DiagnosticAnalyzer {
		static readonly LocalizableString Title = CreateLocalizableResourceString(nameof(SelfReferencingGenericStructStaticFieldTitle));
		static readonly LocalizableString MessageFormat = CreateLocalizableResourceString(nameof(SelfReferencingGenericStructStaticFieldMessageFormat));
		static readonly LocalizableString Description = CreateLocalizableResourceString(nameof(SelfReferencingGenericStructStaticFieldDescription));

		public const string DiagnosticId = "CRYVEEW2001";
		const string Category = "Design";
		static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true, Description);
		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

		public override void Initialize(AnalysisContext context) {
			if (context is null) throw new ArgumentNullException(nameof(context));
			context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
			context.EnableConcurrentExecution();

			context.RegisterCompilationStartAction(InitializeWorker);
		}

		void InitializeWorker(CompilationStartAnalysisContext context) {
			context.RegisterSymbolAction(Analyze, SymbolKind.Field, SymbolKind.Property);
		}

		public static void Analyze(SymbolAnalysisContext context) {
			if (context.Symbol.ContainingType is not INamedTypeSymbol containingType || containingType.TypeKind != TypeKind.Struct)
				return;
			if (context.Symbol is IFieldSymbol field) {
				if (Analyze(field.Type, containingType)) {
					context.ReportDiagnostic(Diagnostic.Create(Rule, field.Locations.FirstOrDefault()));
				}
			}
			else if (context.Symbol is IPropertySymbol property && IsAutoProperty(property)) {
				if (Analyze(property.Type, containingType)) {
					context.ReportDiagnostic(Diagnostic.Create(Rule, property.Locations.FirstOrDefault()));
				}
			}
		}
		static bool IsAutoProperty(IPropertySymbol property) => property.ContainingType.GetMembers().OfType<IFieldSymbol>().Any(f => f.IsImplicitlyDeclared && SymbolEqualityComparer.Default.Equals(property, f.AssociatedSymbol));

		static bool Analyze(ITypeSymbol type, INamedTypeSymbol containingType, bool isRoot = true) {
			if (type is not INamedTypeSymbol namedType)
				return false;
			if (isRoot && !namedType.IsGenericType)
				return false;
			if (SymbolEqualityComparer.Default.Equals(namedType.OriginalDefinition, containingType)) {
				if (!isRoot)
					return true;
				foreach (var arg in namedType.TypeArguments) {
					if (arg is ITypeParameterSymbol)
						continue;
					return true;
				}
			}
			else {
				foreach (var arg in namedType.TypeArguments) {
					if (arg is not INamedTypeSymbol namedTypeArg)
						continue;
					if (!Analyze(namedTypeArg, containingType, false))
						continue;
					return true;
				}
			}
			return false;
		}
	}
}
