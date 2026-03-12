using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Cryville.EEW.Analyzer {
	using static ResourceHelpers;
	using static Resources;

	[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
	public class NoInliningPublicResources : DiagnosticAnalyzer {
		static readonly LocalizableString Title = CreateLocalizableResourceString(nameof(NoInliningPublicResourcesTitle));
		static readonly LocalizableString MessageFormat = CreateLocalizableResourceString(nameof(NoInliningPublicResourcesMessageFormat));
		static readonly LocalizableString Description = CreateLocalizableResourceString(nameof(NoInliningPublicResourcesDescription));

		public const string DiagnosticId = "CRYVEEW1002";
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
			readonly HashSet<ITypeSymbol> ResourceTypes;
			readonly ITypeSymbol MethodImplAttributeType;

			public InternalAnalyzer(Compilation compilation) {
				ResourceTypes = new([
					compilation.GetType("Cryville.EEW.LocalizedResource"),
					compilation.GetType("Cryville.EEW.LocalizableResource"),
				], SymbolEqualityComparer.Default);
				MethodImplAttributeType = compilation.GetType("System.Runtime.CompilerServices.MethodImplAttribute");
			}

			public void Analyze(OperationAnalysisContext context) {
				if (context.Operation is not IObjectCreationOperation creationOp)
					return;
				if (creationOp.Type is not ITypeSymbol type || !ResourceTypes.Contains(type))
					return;
				var containingSymbol = context.ContainingSymbol;
				if (!(
					(containingSymbol is IFieldSymbol fieldSymbol && ResourceTypes.Contains(fieldSymbol.Type)) ||
					(containingSymbol is IMethodSymbol methodSymbol && ResourceTypes.Contains(methodSymbol.ReturnType)) ||
					(containingSymbol is IPropertySymbol propertySymbol && ResourceTypes.Contains(propertySymbol.Type))
				))
					return;
				if (containingSymbol.DeclaredAccessibility != Accessibility.Public)
					return;
				if (containingSymbol.GetAttributes().Any(attr => {
					if (!SymbolEqualityComparer.Default.Equals(attr.AttributeClass, MethodImplAttributeType))
						return false;
					if (attr.ConstructorArguments.Length == 0)
						return false;
					var attrArg = attr.ConstructorArguments[0];
					return ((MethodImplAttributes)attrArg.Value! & MethodImplAttributes.NoInlining) != 0;
				}))
					return;
				context.ReportDiagnostic(Diagnostic.Create(Rule, containingSymbol.Locations.FirstOrDefault(), containingSymbol.ToDisplayString(SymbolDisplayFormat.CSharpShortErrorMessageFormat)));
			}
		}
	}
}
