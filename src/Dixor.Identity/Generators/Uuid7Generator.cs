namespace Dixor.Identity.Generators;

/// <summary>
/// Generates UUID version 7 identifiers.
/// </summary>
public static class Uuid7Generator
{
    /// <summary>
    /// Generates a UUID version 7 identifier.
    /// </summary>
    /// <returns>A new UUIDv7.</returns>
    public static Guid NewGuid()
    {
        return Guid.CreateVersion7();
    }
}