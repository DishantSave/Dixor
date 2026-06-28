using Dixor.Identity.UUID7.Internal;
using Xunit;

namespace Dixor.Identity.Tests.UUID7;

/// <summary>
/// Contains unit tests for <see cref="Uuid7Generator"/>.
/// </summary>
public sealed class Uuid7GeneratorTests
{
    /// <summary>
    /// Verifies that <see cref="Uuid7Generator.Generate()"/>
    /// returns a non-empty GUID.
    /// </summary>
    [Fact]
    public void Generate_Should_Return_NonEmptyGuid()
    {
        // Act
        Guid guid = Uuid7Generator.Generate();

        // Assert
        Assert.NotEqual(Guid.Empty, guid);
    }

    /// <summary>
    /// Verifies that multiple generated UUIDs are unique.
    /// </summary>
    [Fact]
    public void Generate_Should_Return_UniqueGuids()
    {
        // Act
        Guid guid1 = Uuid7Generator.Generate();
        Guid guid2 = Uuid7Generator.Generate();

        // Assert
        Assert.NotEqual(guid1, guid2);
    }

    /// <summary>
    /// Verifies that generating a UUID with a specific timestamp
    /// returns a non-empty GUID.
    /// </summary>
    [Fact]
    public void Generate_WithTimestamp_Should_Return_NonEmptyGuid()
    {
        // Arrange
        DateTimeOffset timestamp = DateTimeOffset.UtcNow;

        // Act
        Guid guid = Uuid7Generator.Generate(timestamp);

        // Assert
        Assert.NotEqual(Guid.Empty, guid);
    }

    /// <summary>
    /// Verifies that UUIDs generated using the same timestamp
    /// remain unique because of their random component.
    /// </summary>
    [Fact]
    public void Generate_WithSameTimestamp_Should_Return_UniqueGuids()
    {
        // Arrange
        DateTimeOffset timestamp = DateTimeOffset.UtcNow;

        // Act
        Guid guid1 = Uuid7Generator.Generate(timestamp);
        Guid guid2 = Uuid7Generator.Generate(timestamp);

        // Assert
        Assert.NotEqual(guid1, guid2);
    }

    /// <summary>
    /// Verifies that an exception is thrown when a timestamp
    /// earlier than the Unix epoch is supplied.
    /// </summary>
    [Fact]
    public void Generate_WithTimestampBeforeUnixEpoch_Should_Throw()
    {
        // Arrange
        DateTimeOffset timestamp =
            DateTimeOffset.UnixEpoch.AddMilliseconds(-1);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(
            () => Uuid7Generator.Generate(timestamp));
    }

    /// <summary>
    /// Verifies that generated UUIDs contain
    /// version 7 information.
    /// </summary>
    [Fact]
    public void Generate_Should_SetVersionTo7()
    {
        // Act
        Guid guid = Uuid7MonotonicGenerator.Generate();

        byte[] bytes = guid.ToByteArray();

        int version = (bytes[6] >> 4) & 0x0F;

        // Assert
        Assert.Equal(7, version);
    }

    /// <summary>
    /// Verifies that generated UUIDs contain the RFC 9562 variant.
    /// </summary>
    [Fact]
    public void Generate_Should_SetRfcVariant()
    {
        // Act
        Guid guid = Uuid7Generator.Generate();

        byte variantByte = guid.ToByteArray()[8];

        // Variant bits should be 10xxxxxx.
        int variant = (variantByte >> 6) & 0b11;

        // Assert
        Assert.Equal(0b10, variant);
    }

    /// <summary>
    /// Verifies that UUIDs generated with increasing timestamps
    /// are different.
    /// </summary>
    [Fact]
    public void Generate_WithIncreasingTimestamps_Should_ReturnDifferentGuids()
    {
        // Arrange
        DateTimeOffset timestamp1 = DateTimeOffset.UtcNow;
        DateTimeOffset timestamp2 = timestamp1.AddMilliseconds(1);

        // Act
        Guid guid1 = Uuid7Generator.Generate(timestamp1);
        Guid guid2 = Uuid7Generator.Generate(timestamp2);

        // Assert
        Assert.NotEqual(guid1, guid2);
    }

    /// <summary>
    /// Verifies that UUID generation succeeds for various timestamps.
    /// </summary>
    /// <param name="year">
    /// The year used to construct the timestamp.
    /// </param>
    [Theory]
    [InlineData(1970)]
    [InlineData(2000)]
    [InlineData(2025)]
    [InlineData(2100)]
    public void Generate_WithVariousTimestamps_Should_Succeed(
        int year)
    {
        // Arrange
        DateTimeOffset timestamp =
            new(year, 1, 1, 0, 0, 0, TimeSpan.Zero);

        // Act
        Guid guid = Uuid7Generator.Generate(timestamp);

        // Assert
        Assert.NotEqual(Guid.Empty, guid);
    }

    /// <summary>
    /// Verifies that a large number of generated UUIDs are unique.
    /// </summary>
    [Fact]
    public void Generate_MultipleGuids_Should_AllBeUnique()
    {
        // Arrange
        const int count = 1000;

        // Act
        List<Guid> guids = new(count);

        for (int i = 0; i < count; i++)
        {
            guids.Add(Uuid7Generator.Generate());
        }

        // Assert
        Assert.Equal(count, guids.Distinct().Count());
    }
}