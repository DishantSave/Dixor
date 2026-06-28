using System.Text.RegularExpressions;

namespace Dixor.Identity.UUID7.Internal;

/// <summary>
/// Provides functionality for extracting and parsing UUID values
/// from arbitrary input strings.
///
/// <para>
/// This parser searches the supplied input for the first occurrence
/// of a valid UUID in the standard canonical format:
/// </para>
///
/// <code>
/// xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
/// </code>
///
/// <para>
/// The parser supports UUID values embedded within larger strings,
/// such as formatted text, log messages, or prefixed identifiers.
/// </para>
/// </summary>
internal static class Uuid7Parser
{
    /// <summary>
    /// Represents the regular expression used to locate UUID values
    /// in their canonical textual representation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The expression matches UUID strings in the following format:
    /// </para>
    ///
    /// <code>
    /// xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
    /// </code>
    ///
    /// <para>
    /// The expression is compiled to improve performance when parsing
    /// multiple values.
    /// </para>
    /// </remarks>
    private static readonly Regex GuidRegex =
        new(
            @"[0-9a-fA-F]{8}-" +
            @"[0-9a-fA-F]{4}-" +
            @"[0-9a-fA-F]{4}-" +
            @"[0-9a-fA-F]{4}-" +
            @"[0-9a-fA-F]{12}",
            RegexOptions.Compiled);

    /// <summary>
    /// Extracts and parses the first UUID found within the specified input.
    /// </summary>
    /// <param name="input">
    /// The input string that may contain a UUID.
    /// </param>
    /// <returns>
    /// A <see cref="Guid"/> instance representing the parsed UUID.
    /// </returns>
    /// <exception cref="FormatException">
    /// Thrown when the input does not contain a valid UUID.
    /// </exception>
    /// <remarks>
    /// <para>
    /// This method searches the input for the first occurrence of a UUID
    /// in canonical textual format.
    /// </para>
    ///
    /// <para>
    /// If no valid UUID is found, a <see cref="FormatException"/> is thrown.
    /// </para>
    /// </remarks>
    public static Guid Parse(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (TryParse(input, out Guid guid))
            return guid;

        throw new FormatException(
            "Input does not contain a valid UUID.");
    }

    /// <summary>
    /// Attempts to extract and parse the first UUID found within
    /// the specified input.
    /// </summary>
    /// <param name="input">
    /// The input string that may contain a UUID.
    /// </param>
    /// <param name="guid">
    /// When this method returns, contains the parsed UUID if parsing
    /// succeeded; otherwise, contains <see cref="Guid.Empty"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if a valid UUID was successfully found
    /// and parsed; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method searches the supplied input for the first UUID
    /// matching the canonical textual representation.
    /// </para>
    ///
    /// <para>
    /// Unlike <see cref="Parse(string)"/>, this method does not throw
    /// an exception when parsing fails.
    /// </para>
    /// </remarks>
    public static bool TryParse(
        string input,
        out Guid guid)
    {
        ArgumentNullException.ThrowIfNull(input);

        guid = Guid.Empty;

        Match match = GuidRegex.Match(input);

        if (!match.Success)
            return false;

        return Guid.TryParse(
            match.Value,
            out guid);
    }
}