using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Immutable;

namespace Cryville.EEW.Analyzer {
	using static ResourceHelpers;
	using static Resources;

	[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
	public class AvoidSingleUseOfTagTypeKey : DiagnosticAnalyzer {
		static readonly LocalizableString Title = CreateLocalizableResourceString(nameof(AvoidSingleUseOfTagTypeKeyTitle));
		static readonly LocalizableString MessageFormat = CreateLocalizableResourceString(nameof(AvoidSingleUseOfTagTypeKeyMessageFormat));
		static readonly LocalizableString Description = CreateLocalizableResourceString(nameof(AvoidSingleUseOfTagTypeKeyDescription));

		public const string DiagnosticId = "CRYVEEW3001";
		const string Category = "Performance";
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
				context.RegisterOperationAction(ia.Analyze, OperationKind.Conversion);
			}
			catch (TypeLoadException) { }
		}

		sealed class InternalAnalyzer {
			readonly INamedTypeSymbol TagTypeKeyType;
			readonly INamedTypeSymbol StringType;

			public InternalAnalyzer(Compilation compilation) {
				TagTypeKeyType = compilation.GetType("Cryville.EEW.TagTypeKey");
				StringType = compilation.GetType("System.String");
			}

			public void Analyze(OperationAnalysisContext context) {
				if (context.Operation is not IConversionOperation conversionOp)
					return;
				if (
					conversionOp.Operand is ILiteralOperation operand &&
					SymbolEqualityComparer.Default.Equals(operand.Type, StringType) &&
					SymbolEqualityComparer.Default.Equals(conversionOp.Type, TagTypeKeyType) &&
					IsLocalUseConversion(conversionOp)
				) {
					context.ReportDiagnostic(Diagnostic.Create(Rule, conversionOp.Syntax.GetLocation(), operand.ConstantValue));
				}
			}
			static bool IsLocalUseConversion(IConversionOperation conversionOp) {
				if (conversionOp.Parent is IFieldInitializerOperation or IPropertyInitializerOperation)
					return false;
				if (conversionOp.Parent is IAssignmentOperation assignmentOp)
					return assignmentOp.Target is not IMemberReferenceOperation;
				return true;
			}
		}
	}
}
