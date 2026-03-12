using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Immutable;

namespace Cryville.EEW.Analyzer {
	using static ResourceHelpers;
	using static Resources;

	[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
	public class SpecifyAccuracyOrder : DiagnosticAnalyzer {
		static readonly LocalizableString Title = CreateLocalizableResourceString(nameof(SpecifyAccuracyOrderTitle));
		static readonly LocalizableString MessageFormat = CreateLocalizableResourceString(nameof(SpecifyAccuracyOrderMessageFormat));
		static readonly LocalizableString Description = CreateLocalizableResourceString(nameof(SpecifyAccuracyOrderDescription));

		public const string DiagnosticId = "CRYVEEW1001";
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
			readonly INamedTypeSymbol ReportPropertyType;
			readonly ISymbol AccuracyOrderProperty;

			public InternalAnalyzer(Compilation compilation) {
				ReportPropertyType = compilation.GetType("Cryville.EEW.Report.ReportProperty");
				AccuracyOrderProperty = ReportPropertyType.GetSingleMember("AccuracyOrder");
			}

			public void Analyze(OperationAnalysisContext context) {
				if (context.Operation is not IObjectCreationOperation creationOp)
					return;
				if (!SymbolEqualityComparer.Default.Equals(creationOp.Type, ReportPropertyType))
					return;
				if (creationOp.Initializer is IObjectOrCollectionInitializerOperation initializerOp) {
					foreach (var op in initializerOp.Initializers) {
						if (op is not IAssignmentOperation assignmentOp)
							continue;
						if (assignmentOp.Target is not IPropertyReferenceOperation propertyReferenceOp)
							continue;
						if (SymbolEqualityComparer.Default.Equals(propertyReferenceOp.Member, AccuracyOrderProperty))
							return;
					}
				}
				context.ReportDiagnostic(Diagnostic.Create(Rule, creationOp.Syntax.GetLocation()));
			}
		}
	}
}
