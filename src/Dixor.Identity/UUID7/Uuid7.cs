using Dixor.Identity.UUID7.Internal;

namespace Dixor.Identity.UUID7;

/// <summary>
/// Provides the primary public API for generating, formatting,
/// parsing, validating, and inspecting RFC 9562 UUID version 7
/// identifiers.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="Uuid7"/> serves as the main entry point for all
/// UUID version 7 operations provided by the
/// <c>Dixor.Identity</c> library.
/// </para>
///
/// <para>
/// The API exposes functionality for:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Generating RFC 9562 compliant UUIDv7 values.
/// </description>
/// </item>
/// <item>
/// <description>
/// Generating monotonic UUIDv7 values.
/// </description>
/// </item>
/// <item>
/// <description>
/// Efficient batch generation of multiple UUIDs.
/// </description>
/// </item>
/// <item>
/// <description>
/// Extracting embedded timestamps.
/// </description>
/// </item>
/// <item>
/// <description>
/// Formatting identifiers with prefixes and suffixes.
/// </description>
/// </item>
/// <item>
/// <description>
/// Parsing UUID values from arbitrary text.
/// </description>
/// </item>
/// <item>
/// <description>
/// Validating UUID version 7 identifiers.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// UUID version 7 identifiers are time-ordered and therefore
/// provide significantly better database index locality compared
/// to randomly generated UUID version 4 values.
/// </para>
///
/// <para>
/// All generated identifiers conform to
/// <see href="https://www.rfc-editor.org/rfc/rfc9562">
/// RFC 9562
/// </see>.
/// </para>
/// </remarks>
public static class Uuid7
{
    /// <summary>
    /// Generates a new RFC 9562 compliant UUID version 7 value.
    /// </summary>
    /// <returns>
    /// A newly generated UUID version 7 identifier.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The generated UUID contains:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// A Unix timestamp represented in milliseconds.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Version information identifying the UUID as version 7.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Cryptographically secure random bits.
    /// </description>
    /// </item>
    /// </list>
    ///
    /// <para>
    /// UUID version 7 values are naturally sortable by creation
    /// time and are well suited for database primary keys,
    /// distributed systems, and event sourcing scenarios.
    /// </para>
    /// </remarks>
    public static Guid NewGuid()
        => Uuid7Generator.Generate();

    /// <summary>
    /// Generates a UUID version 7 value using the specified
    /// timestamp.
    /// </summary>
    /// <param name="timestamp">
    /// The timestamp to embed within the generated UUID.
    /// </param>
    /// <returns>
    /// A UUID version 7 identifier containing the supplied
    /// timestamp.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This overload is useful for:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// Historical data migration.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Event replay systems.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Testing and simulation scenarios.
    /// </description>
    /// </item>
    /// </list>
    ///
    /// <para>
    /// The timestamp is converted to Unix time in milliseconds
    /// before being encoded into the UUID.
    /// </para>
    /// </remarks>
    public static Guid NewGuid(DateTimeOffset timestamp)
        => Uuid7Generator.Generate(timestamp);

    /// <summary>
    /// Generates a monotonic UUID version 7 identifier.
    /// </summary>
    /// <returns>
    /// A monotonic UUID version 7 value.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Monotonic UUID generation guarantees ordering for
    /// identifiers generated within the same millisecond.
    /// </para>
    ///
    /// <para>
    /// This is particularly beneficial for:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// High-throughput systems.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Database clustered indexes.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Event sourcing systems.
    /// </description>
    /// </item>
    /// </list>
    ///
    /// <para>
    /// Monotonic UUIDs preserve insertion order even when many
    /// identifiers are generated during the same timestamp
    /// interval.
    /// </para>
    /// </remarks>
    public static Guid NewMonotonicGuid()
        => Uuid7MonotonicGenerator.Generate();

    /// <summary>
    /// Generates multiple UUID version 7 identifiers in a single
    /// operation.
    /// </summary>
    /// <param name="count">
    /// The number of UUID values to generate.
    /// </param>
    /// <returns>
    /// A sequence containing the generated UUID values.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="count"/> is less than or
    /// equal to zero.
    /// </exception>
    /// <remarks>
    /// <para>
    /// Batch generation can improve performance when large
    /// numbers of identifiers are required.
    /// </para>
    ///
    /// <para>
    /// This method is commonly used for:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// Bulk inserts.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Message processing pipelines.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Distributed batch workloads.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    public static IEnumerable<Guid> NewGuids(int count)
        => Uuid7BatchGenerator.Generate(count);

    /// <summary>
    /// Extracts the timestamp embedded within a UUID version 7
    /// identifier.
    /// </summary>
    /// <param name="guid">
    /// The UUID version 7 value from which to extract the
    /// timestamp.
    /// </param>
    /// <returns>
    /// The timestamp embedded within the UUID.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="guid"/> is not a valid
    /// UUID version 7 value.
    /// </exception>
    /// <remarks>
    /// UUID version 7 stores Unix time in milliseconds within
    /// the first 48 bits of the identifier.
    /// </remarks>
    public static DateTimeOffset GetTimestamp(Guid guid)
        => Uuid7TimestampExtractor.Extract(guid);

    /// <summary>
    /// Attempts to extract the timestamp embedded within a UUID
    /// version 7 identifier.
    /// </summary>
    /// <param name="guid">
    /// The UUID to inspect.
    /// </param>
    /// <param name="timestamp">
    /// When this method returns, contains the extracted
    /// timestamp if extraction succeeded; otherwise, contains
    /// the default value.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if timestamp extraction succeeded;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// Unlike <see cref="GetTimestamp(Guid)"/>, this method does
    /// not throw exceptions when extraction fails.
    /// </remarks>
    public static bool TryGetTimestamp(
        Guid guid,
        out DateTimeOffset timestamp)
        => Uuid7TimestampExtractor.TryExtract(
            guid,
            out timestamp);

    /// <summary>
    /// Retrieves the embedded Unix timestamp represented as
    /// milliseconds since the Unix epoch.
    /// </summary>
    /// <param name="guid">
    /// The UUID version 7 identifier.
    /// </param>
    /// <returns>
    /// The embedded Unix timestamp expressed as milliseconds
    /// since 1970-01-01T00:00:00Z.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="guid"/> is not a valid
    /// UUID version 7 value.
    /// </exception>
    public static long GetUnixTimeMilliseconds(Guid guid)
        => Uuid7TimestampExtractor
            .Extract(guid)
            .ToUnixTimeMilliseconds();

    /// <summary>
    /// Determines whether the supplied GUID is a UUID version 7
    /// value.
    /// </summary>
    /// <param name="guid">
    /// The GUID to validate.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the GUID is a UUID version 7
    /// identifier; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsUuid7(Guid guid)
        => Uuid7Validator.IsUuid7(guid);

    /// <summary>
    /// Determines whether the supplied input contains a valid
    /// UUID version 7 value.
    /// </summary>
    /// <param name="input">
    /// The input string to validate.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the input contains a valid
    /// UUID version 7 identifier; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsValid(string input)
        => Uuid7Validator.IsValid(input);

    /// <summary>
    /// Formats a UUID version 7 identifier using optional
    /// prefixes, suffixes, and separators.
    /// </summary>
    /// <param name="guid">
    /// The UUID to format.
    /// </param>
    /// <param name="prefix">
    /// An optional prefix to prepend to the identifier.
    /// </param>
    /// <param name="suffix">
    /// An optional suffix to append to the identifier.
    /// </param>
    /// <param name="separator">
    /// The separator used between identifier components.
    /// Defaults to <c>-</c>.
    /// </param>
    /// <returns>
    /// A formatted identifier string.
    /// </returns>
    /// <remarks>
    /// Example output:
    ///
    /// <code>
    /// ORD-019853a6-35c4-7e58-bf91-cb7d0b1b11d2-EU
    /// </code>
    /// </remarks>
    public static string Format(
        Guid guid,
        string? prefix = null,
        string? suffix = null,
        string separator = "-")
        => Uuid7Formatter.Format(
            guid,
            prefix,
            suffix,
            separator);

    /// <summary>
    /// Generates a new UUID version 7 identifier and immediately
    /// formats it as a string.
    /// </summary>
    /// <param name="prefix">
    /// An optional prefix.
    /// </param>
    /// <param name="suffix">
    /// An optional suffix.
    /// </param>
    /// <param name="separator">
    /// The separator character used between components.
    /// </param>
    /// <returns>
    /// A formatted UUID string.
    /// </returns>
    /// <remarks>
    /// Example:
    ///
    /// <code>
    /// ORD-019853a6-35c4-7e58-bf91-cb7d0b1b11d2-EU
    /// </code>
    /// </remarks>
    public static string NewString(
        string? prefix = null,
        string? suffix = null,
        string separator = "-")
        => Uuid7Formatter.Format(
            NewGuid(),
            prefix,
            suffix,
            separator);

    /// <summary>
    /// Parses a UUID value from the specified input.
    /// </summary>
    /// <param name="input">
    /// The input string containing a UUID value.
    /// </param>
    /// <returns>
    /// The parsed UUID value.
    /// </returns>
    /// <exception cref="FormatException">
    /// Thrown when a UUID cannot be extracted from the input.
    /// </exception>
    public static Guid Parse(string input)
        => Uuid7Parser.Parse(input);

    /// <summary>
    /// Attempts to parse a UUID value from the specified input.
    /// </summary>
    /// <param name="input">
    /// The input string containing a UUID.
    /// </param>
    /// <param name="guid">
    /// When this method returns, contains the parsed UUID if
    /// parsing succeeded; otherwise, contains
    /// <see cref="Guid.Empty"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if parsing succeeded; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static bool TryParse(
        string input,
        out Guid guid)
        => Uuid7Parser.TryParse(input, out guid);
}