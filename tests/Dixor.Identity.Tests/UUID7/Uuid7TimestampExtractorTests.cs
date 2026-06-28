using Dixor.Identity.UUID7.Internal;
using Xunit;

namespace Dixor.Identity.Tests.UUID7;

/// <summary>
/// Contains unit tests for
/// <see cref="Uuid7TimestampExtractor"/>.
/// </summary>
public sealed class Uuid7TimestampExtractorTests
{
    /// <summary>
    /// Verifies that the timestamp can be extracted
    /// from a generated UUIDv7.
    /// </summary>
    [Fact]
    public void Extract_Should_ReturnTimestamp_ForUuid7()
    {
        // Arrange
        DateTimeOffset before =
            DateTimeOffset.UtcNow;

        Guid guid =
            Uuid7Generator.Generate();

        DateTimeOffset after =
            DateTimeOffset.UtcNow;

        // Act
        DateTimeOffset timestamp =
            Uuid7TimestampExtractor.Extract(guid);

        // Assert
        Assert.InRange(
            timestamp,
            before.AddSeconds(-1),
            after.AddSeconds(1));
    }

    /// <summary>
    /// Verifies that extraction returns the exact
    /// timestamp when using a known timestamp.
    /// </summary>
    [Fact]
    public void Extract_Should_ReturnExactTimestamp()
    {
        // Arrange
        DateTimeOffset expected =
            new(
                2026,
                1,
                1,
                12,
                30,
                45,
                TimeSpan.Zero);

        Guid guid =
            Uuid7Generator.Generate(expected);

        // Act
        DateTimeOffset actual =
            Uuid7TimestampExtractor.Extract(guid);

        // Assert
        Assert.Equal(
            expected.ToUnixTimeMilliseconds(),
            actual.ToUnixTimeMilliseconds());
    }

    /// <summary>
    /// Verifies that Extract throws when
    /// a non-UUIDv7 value is supplied.
    /// </summary>
    [Fact]
    public void Extract_Should_ThrowArgumentException_ForNonUuid7()
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => Uuid7TimestampExtractor.Extract(guid));
    }

    /// <summary>
    /// Verifies that Extract throws when
    /// Guid.Empty is supplied.
    /// </summary>
    [Fact]
    public void Extract_Should_ThrowArgumentException_ForEmptyGuid()
    {
        // Arrange
        Guid guid = Guid.Empty;

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => Uuid7TimestampExtractor.Extract(guid));
    }

    /// <summary>
    /// Verifies that TryExtract succeeds
    /// for UUIDv7 values.
    /// </summary>
    [Fact]
    public void TryExtract_Should_ReturnTrue_ForUuid7()
    {
        // Arrange
        Guid guid =
            Uuid7Generator.Generate();

        // Act
        bool result =
            Uuid7TimestampExtractor.TryExtract(
                guid,
                out DateTimeOffset timestamp);

        // Assert
        Assert.True(result);
        Assert.NotEqual(default, timestamp);
    }

    /// <summary>
    /// Verifies that TryExtract returns false
    /// for non-UUIDv7 values.
    /// </summary>
    [Fact]
    public void TryExtract_Should_ReturnFalse_ForNonUuid7()
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act
        bool result =
            Uuid7TimestampExtractor.TryExtract(
                guid,
                out DateTimeOffset timestamp);

        // Assert
        Assert.False(result);
        Assert.Equal(default, timestamp);
    }

    /// <summary>
    /// Verifies that TryExtract returns false
    /// for Guid.Empty.
    /// </summary>
    [Fact]
    public void TryExtract_Should_ReturnFalse_ForEmptyGuid()
    {
        // Arrange
        Guid guid = Guid.Empty;

        // Act
        bool result =
            Uuid7TimestampExtractor.TryExtract(
                guid,
                out DateTimeOffset timestamp);

        // Assert
        Assert.False(result);
        Assert.Equal(default, timestamp);
    }

    /// <summary>
    /// Verifies that multiple generated UUIDv7 values
    /// produce extractable timestamps.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    public void Extract_Should_Succeed_ForMultipleGeneratedValues(
        int count)
    {
        // Arrange
        IEnumerable<Guid> guids =
            Enumerable.Range(0, count)
                .Select(_ => Uuid7Generator.Generate());

        // Act & Assert
        foreach (Guid guid in guids)
        {
            DateTimeOffset timestamp =
                Uuid7TimestampExtractor.Extract(guid);

            Assert.NotEqual(default, timestamp);
        }
    }

    /// <summary>
    /// Verifies that timestamps extracted from
    /// sequential UUIDs are non-decreasing.
    /// </summary>
    [Fact]
    public void Extract_Should_ReturnMonotonicTimestamps()
    {
        // Arrange
        Guid first =
            Uuid7Generator.Generate();

        Thread.Sleep(10);

        Guid second =
            Uuid7Generator.Generate();

        // Act
        DateTimeOffset firstTimestamp =
            Uuid7TimestampExtractor.Extract(first);

        DateTimeOffset secondTimestamp =
            Uuid7TimestampExtractor.Extract(second);

        // Assert
        Assert.True(
            secondTimestamp >= firstTimestamp);
    }

    /// <summary>
    /// Verifies that TryExtract returns the same
    /// timestamp as Extract.
    /// </summary>
    [Fact]
    public void TryExtract_Should_MatchExtract()
    {
        // Arrange
        Guid guid =
            Uuid7Generator.Generate();

        // Act
        DateTimeOffset expected =
            Uuid7TimestampExtractor.Extract(guid);

        bool result =
            Uuid7TimestampExtractor.TryExtract(
                guid,
                out DateTimeOffset actual);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual);
    }
}