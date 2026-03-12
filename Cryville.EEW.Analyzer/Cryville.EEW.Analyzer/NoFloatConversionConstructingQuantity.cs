using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Cryville.EEW.Analyzer {
	using static ResourceHelpers;
	using static Resources;

	[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
	public class NoFloatConversionConstructingQuantity : DiagnosticAnalyzer {
		static readonly LocalizableString Title = CreateLocalizableResourceString(nameof(NoFloatConversionConstructingQuantityTitle));
		static readonly LocalizableString MessageFormat = CreateLocalizableResourceString(nameof(NoFloatConversionConstructingQuantityMessageFormat));
		static readonly LocalizableString Description = CreateLocalizableResourceString(nameof(NoFloatConversionConstructingQuantityDescription));

		public const string DiagnosticId = "CRYVEEW1003";
		const string Category = "Usage";
		static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true, Description);
		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

		public override void Initialize(AnalysisContext context) {
			if (context is null) throw new ArgumentNullException(nameof(context));
			context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
			context.EnableConcurrentExecution();

			context.RegisterCompilationStartAction(InitializeWorker);
		}

		void InitializeWorker(CompilationStartAnalysisContext context) {
			try {
				var ia = new InternalAnalyzer(context.Compilation);
				context.RegisterOperationAction(ia.Analyze, OperationKind.ObjectCreation);
			}
			catch (TypeLoadException) { }
		}

		sealed class InternalAnalyzer {
			readonly HashSet<ITypeSymbol> QuantityTypes;
			readonly INamedTypeSymbol SingleType;
			readonly INamedTypeSymbol DoubleType;

			public InternalAnalyzer(Compilation compilation) {
				QuantityTypes = new([
					compilation.GetType("Cryville.Measure.QuantityUnc"),
					compilation.GetType("Cryville.Measure.QuantityInc"),
				], SymbolEqualityComparer.Default);
				SingleType = compilation.GetType("System.Single");
				DoubleType = compilation.GetType("System.Double");
			}

			public void Analyze(OperationAnalysisContext context) {
				if (context.Operation is not IObjectCreationOperation creationOp)
					return;
				if (creationOp.Type is not ITypeSymbol type || !QuantityTypes.Contains(type))
					return;
				foreach (var op in creationOp.Arguments) {
					if (op.Value is not IConversionOperation conversionOp)
						continue;
					if (
						SymbolEqualityComparer.Default.Equals(conversionOp.Operand.Type, SingleType) &&
						SymbolEqualityComparer.Default.Equals(conversionOp.Type, DoubleType)
					) {
						context.ReportDiagnostic(Diagnostic.Create(Rule, conversionOp.Syntax.GetLocation()));
					}
				}
			}
		}
	}
}
