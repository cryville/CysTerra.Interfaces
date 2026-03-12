using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Immutable;

namespace Cryville.EEW.Analyzer {
	using static ResourceHelpers;
	using static Resources;

	[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
	public class NoImplicitDateTimeOffsetConversion : DiagnosticAnalyzer {
		static readonly LocalizableString Title = CreateLocalizableResourceString(nameof(NoImplicitDateTimeOffsetConversionTitle));
		static readonly LocalizableString MessageFormat = CreateLocalizableResourceString(nameof(NoImplicitDateTimeOffsetConversionMessageFormat));
		static readonly LocalizableString Description = CreateLocalizableResourceString(nameof(NoImplicitDateTimeOffsetConversionDescription));

		public const string DiagnosticId = "CRYVEEW0003";
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
			context.RegisterOperationAction(ia.Analyze, OperationKind.Conversion);
		}

		sealed class InternalAnalyzer {
			readonly INamedTypeSymbol _dateTimeType;
			readonly INamedTypeSymbol _dateTimeOffsetType;
			public InternalAnalyzer(Compilation compilation) {
				_dateTimeType = compilation.GetType("System.DateTime");
				_dateTimeOffsetType = compilation.GetType("System.DateTimeOffset");
			}

			public void Analyze(OperationAnalysisContext context) {
				if (context.Operation is IConversionOperation conversionOp) {
					if (SymbolEqualityComparer.Default.Equals(conversionOp.Operand.Type, _dateTimeType) && SymbolEqualityComparer.Default.Equals(conversionOp.Type, _dateTimeOffsetType)) {
						context.ReportDiagnostic(Diagnostic.Create(Rule, conversionOp.Syntax.GetLocation()));
					}
				}
			}
		}
	}
}
