using Microsoft.CodeAnalysis;

namespace Dixor.Identity.Analyzers.UUID7;

/// <summary>
/// Provides the collection of diagnostic descriptors supported by
/// the UUID version 7 analyzers.
///
/// <para>
/// This type centralizes all diagnostics reported by the
/// <c>Dixor.Identity.Analyzers</c> package in order to ensure
/// consistency, simplify maintenance and provide a single source
/// of truth for analyzer metadata.
/// </para>
///
/// <para>
/// Each diagnostic descriptor defines the diagnostic identifier,
/// title, message format, category, severity and description
/// associated with a specific analyzer rule.
/// </para>
/// </summary>
internal static class DiagnosticDescriptors
{
    /// <summary>
    /// Represents the diagnostic that is reported when an invalid
    /// batch size is supplied to <c>Uuid7.NewGuids(...)</c>.
    ///
    /// <para>
    /// This diagnostic is reported when a compile-time constant
    /// value less than or equal to zero is passed as the batch
    /// size argument.
    /// </para>
    /// </summary>
    public static readonly DiagnosticDescriptor InvalidBatchSize =
        new(
            id: DiagnosticIds.InvalidBatchSize,
            title: "Invalid UUID batch size",
            messageFormat:
                "The count argument passed to 'Uuid7.NewGuids' must be greater than zero. Use a value greater than zero.",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description:
                "Batch generation count must be greater than zero.");

    /// <summary>
    /// Represents the diagnostic that is reported when a prefix
    /// supplied to <c>Uuid7.Format(...)</c> or
    /// <c>Uuid7.NewString(...)</c> exceeds the maximum supported
    /// length.
    ///
    /// <para>
    /// This diagnostic is only reported when the prefix value is
    /// known at compile time.
    /// </para>
    /// </summary>
    public static readonly DiagnosticDescriptor PrefixTooLong =
        new(
            id: DiagnosticIds.PrefixTooLong,
            title: "Prefix exceeds maximum length",
            messageFormat:
                "The prefix exceeds the maximum supported length of 50 characters",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description:
                "Prefixes supplied to UUID formatting APIs must not exceed 50 characters.");

    /// <summary>
    /// Represents the diagnostic that is reported when a suffix
    /// supplied to <c>Uuid7.Format(...)</c> or
    /// <c>Uuid7.NewString(...)</c> exceeds the maximum supported
    /// length.
    ///
    /// <para>
    /// This diagnostic is only reported when the suffix value is
    /// known at compile time.
    /// </para>
    /// </summary>
    public static readonly DiagnosticDescriptor SuffixTooLong =
        new(
            id: DiagnosticIds.SuffixTooLong,
            title: "Suffix exceeds maximum length",
            messageFormat:
                "The suffix exceeds the maximum supported length of 50 characters",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description:
                "Suffixes supplied to UUID formatting APIs must not exceed 50 characters.");

    /// <summary>
    /// Represents the diagnostic that is reported when an invalid
    /// separator is supplied to <c>Uuid7.Format(...)</c> or
    /// <c>Uuid7.NewString(...)</c>.
    ///
    /// <para>
    /// This diagnostic is reported when the separator:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// Does not consist of exactly one character.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Is not one of the supported separator characters.
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// This diagnostic is only reported when the separator value
    /// is known at compile time.
    /// </para>
    /// </summary>
    public static readonly DiagnosticDescriptor InvalidSeparator =
        new(
            id: DiagnosticIds.InvalidSeparator,
            title: "Invalid separator",
            messageFormat:
                "Separator '{0}' is not supported",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description:
                "Separators supplied to UUID formatting APIs must consist of exactly one supported character.");
}