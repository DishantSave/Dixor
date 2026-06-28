using System.Security.Cryptography;

namespace Dixor.Identity.UUID7.Internal;

/// <summary>
/// Provides functionality for generating monotonic UUID version 7
/// identifiers in accordance with RFC 9562 recommendations.
///
/// <para>
/// Unlike standard UUIDv7 generation, this implementation guarantees
/// monotonic ordering for identifiers generated within the same
/// millisecond by maintaining an internal sequence counter.
/// </para>
///
/// <para>
/// When multiple UUIDs are generated during the same millisecond,
/// the generator increments an internal counter and embeds its value
/// into the UUID, ensuring that each subsequently generated UUID
/// sorts after the previous one.
/// </para>
///
/// <para>
/// This behavior improves index locality and insertion performance
/// when UUIDs are used as primary keys in databases.
/// </para>
///
/// <para>
/// Thread safety is ensured through synchronization, allowing the
/// generator to be safely used concurrently across multiple threads.
/// </para>
///
/// <para>
/// This type is intended for internal use only and is not part of
/// the public API surface.
/// </para>
/// </summary>
internal static class Uuid7MonotonicGenerator
{
    /// <summary>
    /// Synchronizes access to the generator state.
    /// </summary>
    private static readonly object _lock = new();

    /// <summary>
    /// Stores the Unix timestamp, expressed in milliseconds,
    /// associated with the most recently generated UUID.
    /// </summary>
    private static long _lastTimestamp;

    /// <summary>
    /// Stores the sequence counter used to guarantee monotonicity
    /// for UUIDs generated within the same millisecond.
    /// </summary>
    private static ushort _counter;

    /// <summary>
    /// Generates a new monotonic UUID version 7 identifier.
    /// </summary>
    /// <returns>
    /// A newly generated UUID version 7 identifier that is guaranteed
    /// to be monotonically increasing relative to previously generated
    /// UUIDs within the same process.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The generated UUID contains:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// A 48-bit Unix timestamp with millisecond precision.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Cryptographically secure random bits.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// A monotonic sequence counter to guarantee ordering when
    /// multiple UUIDs are generated within the same millisecond.
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// This implementation is thread-safe.
    /// </para>
    /// </remarks>
    public static Guid Generate()
    {
        lock (_lock)
        {
            // Obtain the current Unix timestamp in milliseconds.
            long currentTimestamp =
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // Increment the counter if UUIDs are being generated
            // within the same millisecond; otherwise reset it.
            if (currentTimestamp == _lastTimestamp)
            {
                _counter++;
            }
            else
            {
                _counter = 0;
                _lastTimestamp = currentTimestamp;
            }

            Span<byte> bytes = stackalloc byte[16];

            // Write the 48-bit Unix timestamp in network byte order
            // (big-endian) as required by RFC 9562.
            bytes[0] = (byte)(currentTimestamp >> 40);
            bytes[1] = (byte)(currentTimestamp >> 32);
            bytes[2] = (byte)(currentTimestamp >> 24);
            bytes[3] = (byte)(currentTimestamp >> 16);
            bytes[4] = (byte)(currentTimestamp >> 8);
            bytes[5] = (byte)currentTimestamp;

            // Fill the remaining bytes with cryptographically
            // secure random values.
            RandomNumberGenerator.Fill(bytes[6..]);

            // Set the UUID version to 7.
            //
            // xxxx -> 0111xxxx
            bytes[6] &= 0x0F;
            bytes[6] |= 0x70;

            // Set the RFC 9562 variant bits.
            //
            // xxxxxx -> 10xxxxxx
            bytes[8] &= 0x3F;
            bytes[8] |= 0x80;

            // Store the monotonic counter in the final two bytes.
            // This ensures ordering for UUIDs generated during the
            // same millisecond.
            bytes[14] = (byte)(_counter >> 8);
            bytes[15] = (byte)_counter;

            return GuidByteConverter.ToGuid(bytes);
        }
    }
}