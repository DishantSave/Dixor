namespace Dixor.Identity.UUID7.Internal;

/// <summary>
/// Provides validation functionality for determining whether
/// a value represents a valid UUID version 7 identifier.
/// </summary>
/// <remarks>
/// <para>
/// This class exposes methods for validating both
/// <see cref="Guid"/> instances and string representations
/// of UUID values.
/// </para>
///
/// <para>
/// UUID version 7 is defined by RFC 9562 and stores the
/// version number within the most significant four bits
/// of byte six. A UUID is considered a UUIDv7 when these
/// bits are equal to <c>0x7</c>.
/// </para>
///
/// <para>
/// Validation performed by this class focuses solely on
/// verifying the UUID version field. It assumes that any
/// supplied <see cref="Guid"/> has already been successfully
/// parsed into a valid GUID structure.
/// </para>
/// </remarks>
internal static class Uuid7Validator
{
    /// <summary>
    /// Determines whether the specified <see cref="Guid"/>
    /// represents a UUID version 7 value.
    /// </summary>
    /// <param name="guid">
    /// The <see cref="Guid"/> value to validate.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the supplied value is a
    /// UUID version 7; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// UUID version information is stored within the
    /// most significant nibble of byte six.
    /// </para>
    ///
    /// <para>
    /// This method extracts the UUID bytes and verifies
    /// that the version field is equal to <c>7</c>,
    /// as defined by RFC 9562.
    /// </para>
    /// </remarks>
    public static bool IsUuid7(Guid guid)
    {
        Span<byte> bytes = stackalloc byte[16];

        GuidByteConverter.ToBytes(guid, bytes);

        return (bytes[6] >> 4) == 0x7;
    }

    /// <summary>
    /// Determines whether the specified input contains
    /// a valid UUID version 7 value.
    /// </summary>
    /// <param name="input">
    /// The input string containing a UUID value to validate.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the input contains a valid
    /// UUID version 7 value; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method first attempts to extract and parse a UUID
    /// from the supplied input using
    /// <see cref="Uuid7Parser.TryParse(string, out Guid)"/>.
    /// </para>
    ///
    /// <para>
    /// If parsing succeeds, the resulting UUID is validated
    /// to determine whether it is a UUID version 7 value.
    /// </para>
    ///
    /// <para>
    /// This method returns <see langword="false"/> when:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// The input does not contain a valid UUID.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The parsed UUID is not a UUID version 7 value.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="input"/> is
    /// <see langword="null"/> and the underlying parser
    /// does not support null values.
    /// </exception>
    public static bool IsValid(string input)
    {
        return Uuid7Parser.TryParse(
            input,
            out Guid guid)
            && IsUuid7(guid);
    }
}