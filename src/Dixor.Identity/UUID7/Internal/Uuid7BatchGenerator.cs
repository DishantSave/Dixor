namespace Dixor.Identity.UUID7.Internal;

/// <summary>
/// Provides functionality for generating multiple UUID version 7
/// identifiers in a single operation.
///
/// <para>
/// This type supports deferred batch generation of UUIDv7 values,
/// allowing consumers to efficiently generate large numbers of
/// identifiers without allocating all values in memory at once.
/// </para>
///
/// <para>
/// UUIDs are generated lazily as the returned sequence is enumerated.
/// This makes the implementation memory-efficient and suitable for
/// bulk processing scenarios such as database seeding, test data
/// generation and batch inserts.
/// </para>
///
/// <para>
/// This type is intended for internal use only and is not part of
/// the public API surface.
/// </para>
/// </summary>
internal static class Uuid7BatchGenerator
{
    /// <summary>
    /// Generates the specified number of UUID version 7 identifiers.
    /// </summary>
    /// <param name="count">
    /// The number of UUIDs to generate.
    /// Must be greater than zero.
    /// </param>
    /// <returns>
    /// A lazily evaluated sequence containing the generated UUIDv7 values.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="count"/> is less than or equal to zero.
    /// </exception>
    public static IEnumerable<Guid> Generate(int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(count, 0);

        for (int i = 0; i < count; i++)
        {
            yield return Uuid7Generator.Generate();
        }
    }
}