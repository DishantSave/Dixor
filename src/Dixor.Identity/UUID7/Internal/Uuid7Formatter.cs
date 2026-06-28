namespace Dixor.Identity.UUID7.Internal;

/// <summary>
/// Provides functionality for formatting UUID version 7 identifiers
/// with optional prefixes, suffixes and custom separators.
///
/// <para>
/// This type enables creation of business-friendly identifiers by
/// decorating UUIDv7 values with additional contextual information.
/// </para>
///
/// <para>
/// Examples:
/// <code>
/// Guid id = Uuid7.NewGuid();
///
/// string value1 = Uuid7Formatter.Format(id, "DS", null, "-");
/// // DS-0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10
///
/// string value2 = Uuid7Formatter.Format(id, "DS", "PSA", "_");
/// // DS_0197d8ef-f4c6-7a35-99e3-1d4a7e3c8e10_PSA
/// </code>
/// </para>
///
/// <para>
/// This type is intended for internal use only and is not part of
/// the public API surface.
/// </para>
/// </summary>
internal static class Uuid7Formatter
{
    public const int MaxPrefixLength = 50;
    public const int MaxSuffixLength = 50;

    public static readonly HashSet<char> AllowedSeparators =
    [
        '-',
        ':',
        '~',
        '|',
        '{',
        '}',
        '[',
        ']',
        '<',
        '>',
        '/',
        '\\',
        '_'
    ];

    /// <summary>
    /// Formats the specified UUID version 7 identifier using the
    /// supplied prefix, suffix and separator.
    /// </summary>
    /// <param name="guid">
    /// The UUIDv7 value to format.
    /// </param>
    /// <param name="prefix">
    /// An optional prefix to prepend to the identifier.
    /// If <see langword="null"/>, empty or whitespace, no prefix
    /// is added.
    /// </param>
    /// <param name="suffix">
    /// An optional suffix to append to the identifier.
    /// If <see langword="null"/>, empty or whitespace, no suffix
    /// is added.
    /// </param>
    /// <param name="separator">
    /// The separator used between the UUID and any supplied
    /// prefix or suffix.
    /// </param>
    /// <returns>
    /// A formatted string representation of the UUIDv7 value.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the separator is invalid.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the prefix or suffix exceeds the maximum
    /// supported length.
    /// </exception>
    public static string Format(
        Guid guid,
        string? prefix = null,
        string? suffix = null,
        string separator = "-")
    {
        ValidateSeparator(separator);
        ValidatePrefix(prefix);
        ValidateSuffix(suffix);

        string value = guid.ToString();

        if (!string.IsNullOrWhiteSpace(prefix))
        {
            value = string.Concat(prefix, separator, value);
        }

        if (!string.IsNullOrWhiteSpace(suffix))
        {
            value = string.Concat(value, separator, suffix);
        }

        return value;
    }

    private static void ValidatePrefix(string? prefix)
    {
        if (prefix is not null &&
            prefix.Length > MaxPrefixLength)
        {
            throw new ArgumentOutOfRangeException(
                nameof(prefix),
                prefix.Length,
                $"Prefix length cannot exceed {MaxPrefixLength} characters.");
        }
    }

    private static void ValidateSuffix(string? suffix)
    {
        if (suffix is not null &&
            suffix.Length > MaxSuffixLength)
        {
            throw new ArgumentOutOfRangeException(
                nameof(suffix),
                suffix.Length,
                $"Suffix length cannot exceed {MaxSuffixLength} characters.");
        }
    }

    private static void ValidateSeparator(string separator)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(separator);

        if (separator.Length != 1)
        {
            throw new ArgumentException(
                "Separator must consist of exactly one character.",
                nameof(separator));
        }

        if (!AllowedSeparators.Contains(separator[0]))
        {
            throw new ArgumentException(
                $"Separator '{separator}' is not supported.",
                nameof(separator));
        }
    }
}