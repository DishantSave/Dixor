namespace Dixor.Identity.UUID7.Internal;

/// <summary>
/// Provides functionality for extracting the embedded Unix timestamp
/// from RFC 9562 UUID version 7 values.
/// </summary>
/// <remarks>
/// <para>
/// UUID version 7 stores a 48-bit Unix timestamp, expressed in
/// milliseconds since the Unix epoch, within the first six bytes
/// of the UUID.
/// </para>
///
/// <para>
/// This class exposes methods for extracting that timestamp and
/// converting it into a <see cref="DateTimeOffset"/> instance.
/// </para>
///
/// <para>
/// Only UUID version 7 values are supported. Attempting to extract
/// a timestamp from any other UUID version will result in an
/// exception or a failed operation, depending on the method used.
/// </para>
/// </remarks>
internal static class Uuid7TimestampExtractor
{
    /// <summary>
    /// Extracts the embedded timestamp from the specified UUID version 7 value.
    /// </summary>
    /// <param name="guid">
    /// The UUID version 7 value from which to extract the timestamp.
    /// </param>
    /// <returns>
    /// A <see cref="DateTimeOffset"/> representing the timestamp
    /// embedded within the UUID.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the supplied <paramref name="guid"/> is not a
    /// valid UUID version 7 value.
    /// </exception>
    /// <remarks>
    /// <para>
    /// UUID version 7 stores the Unix timestamp in the first
    /// 48 bits of the UUID.
    /// </para>
    ///
    /// <para>
    /// The extracted timestamp is interpreted as the number of
    /// milliseconds elapsed since the Unix epoch
    /// (<c>1970-01-01T00:00:00Z</c>).
    /// </para>
    ///
    /// <para>
    /// The supplied UUID must conform to RFC 9562 UUID version 7
    /// requirements; otherwise, an <see cref="ArgumentException"/>
    /// is thrown.
    /// </para>
    /// </remarks>
    public static DateTimeOffset Extract(Guid guid)
    {
        if (!Uuid7Validator.IsUuid7(guid))
            throw new ArgumentException(
                "The supplied Guid is not UUIDv7.");

        Span<byte> bytes = stackalloc byte[16];

        GuidByteConverter.ToBytes(guid, bytes);

        long unixTime =
              ((long)bytes[0] << 40)
            | ((long)bytes[1] << 32)
            | ((long)bytes[2] << 24)
            | ((long)bytes[3] << 16)
            | ((long)bytes[4] << 8)
            | bytes[5];

        return DateTimeOffset
            .FromUnixTimeMilliseconds(unixTime);
    }

    /// <summary>
    /// Attempts to extract the embedded timestamp from the specified
    /// UUID version 7 value.
    /// </summary>
    /// <param name="guid">
    /// The UUID value from which to extract the timestamp.
    /// </param>
    /// <param name="timestamp">
    /// When this method returns, contains the extracted timestamp
    /// if extraction succeeded; otherwise, contains the default
    /// value for <see cref="DateTimeOffset"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the timestamp was successfully
    /// extracted; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method provides a non-throwing alternative to
    /// <see cref="Extract(Guid)"/>.
    /// </para>
    ///
    /// <para>
    /// If the supplied UUID is not a valid UUID version 7 value,
    /// or if timestamp extraction fails for any reason,
    /// this method returns <see langword="false"/>.
    /// </para>
    /// </remarks>
    public static bool TryExtract(
        Guid guid,
        out DateTimeOffset timestamp)
    {
        try
        {
            timestamp = Extract(guid);
            return true;
        }
        catch
        {
            timestamp = default;
            return false;
        }
    }
}