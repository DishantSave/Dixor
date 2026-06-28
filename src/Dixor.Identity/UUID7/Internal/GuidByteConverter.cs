namespace Dixor.Identity.UUID7.Internal;

/// <summary>
/// Provides helper methods for converting between
/// RFC 9562 UUID byte ordering and the internal
/// byte representation used by <see cref="Guid"/>.
///
/// <para>
/// The .NET <see cref="Guid"/> type stores the first
/// three UUID components in little-endian format for
/// historical compatibility reasons, whereas RFC 9562
/// defines UUIDs using network byte order (big-endian).
/// </para>
///
/// <para>
/// This class centralizes all byte conversions required
/// by UUIDv7 generation, parsing and timestamp extraction,
/// ensuring consistent RFC-compliant behavior throughout
/// the library.
/// </para>
///
/// <para>
/// This type is intended for internal use only and is
/// not part of the public API surface.
/// </para>
/// </summary>
internal static class GuidByteConverter
{
    /// <summary>
    /// Creates a <see cref="Guid"/> instance from
    /// an RFC-ordered UUID byte sequence.
    /// </summary>
    /// <param name="bytes">
    /// A 16-byte span containing the UUID bytes
    /// in RFC 9562 byte order.
    /// </param>
    /// <returns>
    /// A <see cref="Guid"/> representing the supplied bytes.
    /// </returns>
    public static Guid ToGuid(Span<byte> bytes)
    {
        return new Guid(bytes);
    }

    /// <summary>
    /// Writes the bytes of the specified <see cref="Guid"/>
    /// into the destination span.
    /// </summary>
    /// <param name="guid">
    /// The GUID whose bytes should be written.
    /// </param>
    /// <param name="destination">
    /// The destination span that receives the GUID bytes.
    /// Must be at least 16 bytes long.
    /// </param>
    public static void ToBytes(Guid guid, Span<byte> destination)
    {
        if (!guid.TryWriteBytes(destination))
        {
            throw new ArgumentException(
                "Destination span must be at least 16 bytes long.",
                nameof(destination));
        }
    }
}