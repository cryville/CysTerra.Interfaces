using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Immutable;

namespace Cryville.EEW.Analyzer {
	using static ResourceHelpers;
	using static Resources;

	[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
	public class SpecifyMidpointRounding : DiagnosticAnalyzer {
		static readonly LocalizableString Title = CreateLocalizableResourceString(nameof(SpecifyMidpointRoundingTitle));
		static readonly LocalizableString MessageFormat = CreateLocalizableResourceString(nameof(SpecifyMidpointRoundingMessageFormat));
		static readonly LocalizableString Description = CreateLocalizableResourceString(nameof(SpecifyMidpointRoundingDescription));

		public const string DiagnosticId = "CRYVEEW1004";
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
			var ia = new InternalAnalyzer(context.Compilation);
			context.RegisterOperationAction(ia.Analyze, OperationKind.Invocation);
		}

		sealed class InternalAnalyzer {
			readonly PreferredOverloadFinder _finder;

			public InternalAnalyzer(Compilation compilation) {
				_finder = new(compilation, compilation.GetType("System.MidpointRounding"));
			}

			public void Analyze(OperationAnalysisContext context) {
				if (context.Operation is not IInvocationOperation invocationOp)
					return;
				var method = invocationOp.TargetMethod;
				if (_finder.FindPreferredOverload(method) is not { } result)
					return;
				context.ReportDiagnostic(Diagnostic.Create(Rule, invocationOp.Syntax.GetLocation(), method.ToDisplayString(SymbolDisplayFormat.CSharpShortErrorMessageFormat), result.ToDisplayString(SymbolDisplayFormat.CSharpShortErrorMessageFormat)));
			}
		}
	}
}
