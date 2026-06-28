using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace Dixor.Identity.Analyzers.UUID7;

/// <summary>
/// Analyzes invocations of UUID formatting APIs and reports diagnostics
/// for invalid compile-time argument values.
///
/// <para>
/// The analyzer validates:
/// <list type="bullet">
/// <item>
/// <description>Prefix length does not exceed the supported maximum.</description>
/// </item>
/// <item>
/// <description>Suffix length does not exceed the supported maximum.</description>
/// </item>
/// <item>
/// <description>The separator consists of exactly one supported character.</description>
/// </item>
/// </list>
/// </para>
///
/// <para>
/// Diagnostics are only reported when argument values can be determined
/// at compile time.
/// </para>
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class Uuid7FormatAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// Gets the diagnostics supported by this analyzer.
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
    [
        DiagnosticDescriptors.PrefixTooLong,
        DiagnosticDescriptors.SuffixTooLong,
        DiagnosticDescriptors.InvalidSeparator
    ];

    /// <summary>
    /// Initializes the analyzer and registers analysis actions.
    /// </summary>
    /// <param name="context">
    /// The analysis context used to register analysis callbacks.
    /// </param>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(
            GeneratedCodeAnalysisFlags.None);

        context.EnableConcurrentExecution();

        context.RegisterOperationAction(
            AnalyzeInvocation,
            OperationKind.Invocation);
    }

    private static void AnalyzeInvocation(
        OperationAnalysisContext context)
    {
        if (context.Operation is not IInvocationOperation invocation)
        {
            return;
        }

        if (invocation.TargetMethod.Name is not ("Format" or "NewString"))
        {
            return;
        }

        if (invocation.TargetMethod.ContainingType.Name != "Uuid7")
        {
            return;
        }

        foreach (IArgumentOperation argument in invocation.Arguments)
        {
            if (argument.Parameter is null)
            {
                continue;
            }

            Optional<object?> constantValue =
                argument.Value.ConstantValue;

            if (!constantValue.HasValue ||
                constantValue.Value is not string value)
            {
                continue;
            }

            switch (argument.Parameter.Name)
            {
                case "prefix":
                    AnalyzePrefix(
                        context,
                        argument,
                        value);
                    break;

                case "suffix":
                    AnalyzeSuffix(
                        context,
                        argument,
                        value);
                    break;

                case "separator":
                    AnalyzeSeparator(
                        context,
                        argument,
                        value);
                    break;
            }
        }
    }

    private static void AnalyzePrefix(
        OperationAnalysisContext context,
        IArgumentOperation argument,
        string value)
    {
        if (value.Length <= AnalyzerConstants.MaxPrefixLength)
        {
            return;
        }

        context.ReportDiagnostic(
            Diagnostic.Create(
                DiagnosticDescriptors.PrefixTooLong,
                argument.Syntax.GetLocation()));
    }

    private static void AnalyzeSuffix(
        OperationAnalysisContext context,
        IArgumentOperation argument,
        string value)
    {
        if (value.Length <= AnalyzerConstants.MaxSuffixLength)
        {
            return;
        }

        context.ReportDiagnostic(
            Diagnostic.Create(
                DiagnosticDescriptors.SuffixTooLong,
                argument.Syntax.GetLocation()));
    }

    private static void AnalyzeSeparator(
        OperationAnalysisContext context,
        IArgumentOperation argument,
        string value)
    {
        if (value.Length == 1 &&
            AnalyzerConstants.AllowedSeparators.Contains(value[0]))
        {
            return;
        }

        context.ReportDiagnostic(
            Diagnostic.Create(
                DiagnosticDescriptors.InvalidSeparator,
                argument.Syntax.GetLocation(),
                value));
    }
}