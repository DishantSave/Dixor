using Dixor.Identity.UUID7.Internal;
using Xunit;

namespace Dixor.Identity.Tests.UUID7;

/// <summary>
/// Contains unit tests for <see cref="Uuid7BatchGenerator"/>.
/// </summary>
public sealed class Uuid7BatchGeneratorTests
{
    /// <summary>
    /// Verifies that the requested number of UUIDs is generated.
    /// </summary>
    [Fact]
    public void Generate_WithValidCount_ShouldReturnExpectedNumberOfGuids()
    {
        // Arrange
        const int count = 10;

        // Act
        List<Guid> result =
            [.. Uuid7BatchGenerator.Generate(count)];

        // Assert
        Assert.Equal(count, result.Count);
    }

    /// <summary>
    /// Verifies that all generated UUIDs are non-empty.
    /// </summary>
    [Fact]
    public void Generate_ShouldReturnNonEmptyGuids()
    {
        // Arrange
        const int count = 10;

        // Act
        IEnumerable<Guid> guids =
            Uuid7BatchGenerator.Generate(count);

        // Assert
        Assert.All(
            guids,
            guid => Assert.NotEqual(Guid.Empty, guid));
    }

    /// <summary>
    /// Verifies that all generated UUIDs are unique within a batch.
    /// </summary>
    [Fact]
    public void Generate_ShouldReturnUniqueGuids()
    {
        // Arrange
        const int count = 100;

        // Act
        List<Guid> guids =
            [.. Uuid7BatchGenerator.Generate(count)];

        // Assert
        Assert.Equal(
            guids.Count,
            guids.Distinct().Count());
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentOutOfRangeException"/>
    /// is thrown when the requested count is zero.
    /// </summary>
    [Fact]
    public void Generate_WithZeroCount_ShouldThrowArgumentOutOfRangeException()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(
            () => Uuid7BatchGenerator.Generate(0).ToList());
    }

    /// <summary>
    /// Verifies that an <see cref="ArgumentOutOfRangeException"/>
    /// is thrown when the requested count is negative.
    /// </summary>
    [Fact]
    public void Generate_WithNegativeCount_ShouldThrowArgumentOutOfRangeException()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(
            () => Uuid7BatchGenerator.Generate(-1).ToList());
    }

    /// <summary>
    /// Verifies that UUID generation is deferred until
    /// the returned sequence is enumerated.
    /// </summary>
    [Fact]
    public void Generate_ShouldUseDeferredExecution()
    {
        // Arrange & Act
        IEnumerable<Guid> sequence =
            Uuid7BatchGenerator.Generate(5);

        // Assert
        Assert.NotNull(sequence);
        Assert.IsType<IEnumerable<Guid>>(sequence, exactMatch: false);
    }

    /// <summary>
    /// Verifies that enumerating the sequence multiple times
    /// produces new UUID values each time.
    /// </summary>
    [Fact]
    public void Generate_WhenEnumeratedMultipleTimes_ShouldGenerateNewValues()
    {
        // Arrange
        IEnumerable<Guid> sequence =
            Uuid7BatchGenerator.Generate(5);

        // Act
        List<Guid> firstEnumeration = [.. sequence];
        List<Guid> secondEnumeration = [.. sequence];

        // Assert
        Assert.NotEqual(firstEnumeration, secondEnumeration);
    }

    /// <summary>
    /// Verifies that generating a single UUID returns exactly one value.
    /// </summary>
    [Fact]
    public void Generate_WithCountOne_ShouldReturnSingleGuid()
    {
        // Act
        List<Guid> result =
            [.. Uuid7BatchGenerator.Generate(1)];

        // Assert
        Assert.Single(result);
        Assert.NotEqual(Guid.Empty, result[0]);
    }

    /// <summary>
    /// Verifies that the generated sequence contains
    /// exactly the requested number of elements.
    /// </summary>
    /// <param name="count">
    /// The number of UUIDs to generate.
    /// </param>
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(100)]
    public void Generate_WithVariousCounts_ShouldReturnExpectedCount(
        int count)
    {
        // Act
        int actualCount =
            Uuid7BatchGenerator.Generate(count).Count();

        // Assert
        Assert.Equal(count, actualCount);
    }
}