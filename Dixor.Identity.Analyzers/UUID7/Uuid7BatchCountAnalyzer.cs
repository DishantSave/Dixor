using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Dixor.Identity.Analyzers.UUID7;

/// <summary>
/// Represents a Roslyn analyzer that validates the arguments supplied to
/// <c>Uuid7.NewGuids(...)</c> method invocations.
///
/// <para>
/// This analyzer ensures that the batch size specified when generating
/// multiple UUID version 7 identifiers is greater than zero.
/// </para>
///
/// <para>
/// If a constant value less than or equal to zero is passed to
/// <c>Uuid7.NewGuids(...)</c>, the analyzer reports diagnostic
/// <c>DIXOR001</c>.
/// </para>
///
/// <example>
/// The following example produces a diagnostic:
///
/// <code>
/// var ids = Uuid7.NewGuids(0);
/// </code>
///
/// The following example is valid:
///
/// <code>
/// var ids = Uuid7.NewGuids(100);
/// </code>
/// </example>
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class Uuid7BatchCountAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// Gets the collection of diagnostics supported by this analyzer.
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => [DiagnosticDescriptors.InvalidBatchSize];

    /// <summary>
    /// Initializes the analyzer and registers analysis actions.
    /// </summary>
    /// <param name="context">
    /// The analysis context used to register analysis callbacks.
    /// </param>
    /// <remarks>
    /// This analyzer registers a syntax node action for invocation
    /// expressions in order to inspect calls made to
    /// <c>Uuid7.NewGuids(...)</c>.
    /// </remarks>
    public override void Initialize(AnalysisContext context)
    {
        // Ignore generated code.
        context.ConfigureGeneratedCodeAnalysis(
            GeneratedCodeAnalysisFlags.None);

        // Enable concurrent execution for improved performance.
        context.EnableConcurrentExecution();

        // Register analysis for all method invocations.
        context.RegisterSyntaxNodeAction(
            AnalyzeInvocation,
            Microsoft.CodeAnalysis.CSharp.SyntaxKind.InvocationExpression);
    }

    /// <summary>
    /// Analyzes invocation expressions to determine whether an invalid
    /// batch size has been supplied to <c>Uuid7.NewGuids(...)</c>.
    /// </summary>
    /// <param name="context">
    /// The context containing information about the syntax node being analyzed.
    /// </param>
    /// <remarks>
    /// This method only reports diagnostics when:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// The invoked method is named <c>NewGuids</c>.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The containing type is <c>Uuid7</c>.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The first argument is a compile-time constant integer.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The supplied count is less than or equal to zero.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    private static void AnalyzeInvocation(
        SyntaxNodeAnalysisContext context)
    {
        var invocation =
            (InvocationExpressionSyntax)context.Node;

        // Retrieve the symbol associated with the invocation.

        // Ignore unresolved symbols.
        if (context.SemanticModel
                .GetSymbolInfo(invocation)
                .Symbol is not IMethodSymbol symbol)
            return;

        // Only analyze methods named 'NewGuids'.
        if (symbol.Name != "NewGuids")
            return;

        // Ensure the method belongs to the Uuid7 type.
        if (symbol.ContainingType.Name != "Uuid7")
            return;

        // Ensure that at least one argument was supplied.
        if (invocation.ArgumentList.Arguments.Count == 0)
            return;

        // Obtain the first argument representing the batch size.
        var firstArgument =
            invocation.ArgumentList.Arguments[0];

        // Attempt to evaluate the argument as a compile-time constant.
        var constantValue =
            context.SemanticModel.GetConstantValue(
                firstArgument.Expression);

        // Ignore non-constant expressions.
        if (!constantValue.HasValue)
            return;

        // Report a diagnostic when the count is less than or equal to zero.
        if (constantValue.Value is int count &&
            count <= 0)
        {
            var diagnostic =
                Diagnostic.Create(
                    DiagnosticDescriptors.InvalidBatchSize,
                    firstArgument.GetLocation());

            context.ReportDiagnostic(diagnostic);
        }
    }
}