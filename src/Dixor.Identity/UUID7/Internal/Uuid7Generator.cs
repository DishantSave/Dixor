using System.Security.Cryptography;

namespace Dixor.Identity.UUID7.Internal;

/// <summary>
/// Provides functionality for generating RFC 9562 compliant
/// UUID version 7 identifiers.
///
/// <para>
/// UUID version 7 combines a Unix timestamp expressed in
/// milliseconds with cryptographically secure random data,
/// producing identifiers that are both globally unique and
/// naturally sortable by creation time.
/// </para>
///
/// <para>
/// The generated UUIDs:
/// <list type="bullet">
/// <item>
/// <description>
/// Preserve chronological ordering when compared lexicographically.
/// </description>
/// </item>
/// <item>
/// <description>
/// Contain a 48-bit Unix timestamp with millisecond precision.
/// </description>
/// </item>
/// <item>
/// <description>
/// Use cryptographically secure random bits for uniqueness.
/// </description>
/// </item>
/// <item>
/// <description>
/// Conform to the variant and version requirements defined
/// by RFC 9562.
/// </description>
/// </item>
/// </list>
/// </para>
///
/// <para>
/// This type is intended for internal use only and is not part
/// of the public API surface.
/// </para>
/// </summary>
internal static class Uuid7Generator
{
    /// <summary>
    /// The Unix epoch expressed as a <see cref="DateTimeOffset"/>.
    /// </summary>
    private static readonly DateTimeOffset UnixEpoch =
        DateTimeOffset.UnixEpoch;

    /// <summary>
    /// Generates a new UUID version 7 identifier using the
    /// current UTC timestamp.
    /// </summary>
    /// <returns>
    /// A newly generated UUID version 7 identifier.
    /// </returns>
    /// <remarks>
    /// This method uses <see cref="DateTimeOffset.UtcNow"/>
    /// as the timestamp source.
    /// </remarks>
    public static Guid Generate()
        => Generate(DateTimeOffset.UtcNow);

    /// <summary>
    /// Generates a new UUID version 7 identifier using the
    /// specified timestamp.
    /// </summary>
    /// <param name="timestamp">
    /// The timestamp to embed within the UUID.
    /// </param>
    /// <returns>
    /// A newly generated UUID version 7 identifier containing
    /// the supplied timestamp.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="timestamp"/> occurs before
    /// the Unix epoch.
    /// </exception>
    /// <remarks>
    /// The timestamp is encoded as the first 48 bits of the UUID
    /// in big-endian order as defined by RFC 9562.
    /// </remarks>
    public static Guid Generate(DateTimeOffset timestamp)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(
            timestamp,
            UnixEpoch);

        Span<byte> bytes = stackalloc byte[16];

        // Extract the Unix timestamp in milliseconds.
        long unixTime = timestamp.ToUnixTimeMilliseconds();

        // Write the 48-bit timestamp in network byte order
        // (big-endian).
        bytes[0] = (byte)(unixTime >> 40);
        bytes[1] = (byte)(unixTime >> 32);
        bytes[2] = (byte)(unixTime >> 24);
        bytes[3] = (byte)(unixTime >> 16);
        bytes[4] = (byte)(unixTime >> 8);
        bytes[5] = (byte)unixTime;

        // Fill the remaining bytes with cryptographically
        // secure random data.
        RandomNumberGenerator.Fill(bytes[6..]);

        // Set UUID version to 7.
        //
        // xxxx -> 0111xxxx
        bytes[6] &= 0x0F;
        bytes[6] |= 0x70;

        // Set RFC 9562 variant bits.
        //
        // xxxxxx -> 10xxxxxx
        bytes[8] &= 0x3F;
        bytes[8] |= 0x80;

        return GuidByteConverter.ToGuid(bytes);
    }
}