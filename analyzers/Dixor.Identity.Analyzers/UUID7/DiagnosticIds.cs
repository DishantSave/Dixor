namespace Dixor.Identity.Analyzers.UUID7;

/// <summary>
/// Defines the diagnostic identifiers used by the
/// UUID version 7 analyzers.
///
/// <para>
/// Each constant represents a unique diagnostic rule exposed by
/// the <c>Dixor.Identity.Analyzers</c> package.
/// </para>
///
/// <para>
/// Diagnostic identifiers must remain stable across releases to
/// preserve compatibility with existing codebases, build pipelines
/// and suppression configurations.
/// </para>
/// </summary>
internal static class DiagnosticIds
{
    /// <summary>
    /// Identifies the diagnostic that is reported when an invalid
    /// batch size is supplied to <c>Uuid7.NewGuids(...)</c>.
    /// </summary>
    public const string InvalidBatchSize = "DIXOR001";

    /// <summary>
    /// Identifies the diagnostic that is reported when a prefix
    /// exceeds the maximum supported length.
    /// </summary>
    public const string PrefixTooLong = "DIXOR002";

    /// <summary>
    /// Identifies the diagnostic that is reported when a suffix
    /// exceeds the maximum supported length.
    /// </summary>
    public const string SuffixTooLong = "DIXOR003";

    /// <summary>
    /// Identifies the diagnostic that is reported when an invalid
    /// separator is supplied to a UUID formatting API.
    /// </summary>
    public const string InvalidSeparator = "DIXOR010";
}