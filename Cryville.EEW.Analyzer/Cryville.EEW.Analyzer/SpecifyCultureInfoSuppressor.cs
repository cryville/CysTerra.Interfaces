using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace Cryville.EEW.Analyzer {
	using static ResourceHelpers;
	using static Resources;

	[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
	public class SpecifyCultureInfoSuppressor : DiagnosticSuppressor {
		static readonly LocalizableString Justification = CreateLocalizableResourceString(nameof(SpecifyCultureInfoJustification));

		public const string DiagnosticId = "CRYVEEW0901";
		public const string SuppressedDiagnosticId = "CA1304";
		static readonly SuppressionDescriptor Rule = new(DiagnosticId, SuppressedDiagnosticId, Justification);
		public override ImmutableArray<SuppressionDescriptor> SupportedSuppressions => [Rule];

		public override void ReportSuppressions(SuppressionAnalysisContext context) {
			foreach (var diagnostic in context.ReportedDiagnostics) {
				context.ReportSuppression(Suppression.Create(Rule, diagnostic));
			}
		}
	}
}
