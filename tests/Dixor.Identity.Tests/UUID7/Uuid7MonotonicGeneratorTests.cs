using Dixor.Identity.UUID7.Internal;
using System.Collections.Concurrent;
using Xunit;

namespace Dixor.Identity.Tests.UUID7;

/// <summary>
/// Contains unit tests for
/// <see cref="Uuid7MonotonicGenerator"/>.
/// </summary>
public sealed class Uuid7MonotonicGeneratorTests
{
    /// <summary>
    /// Verifies that the generator returns a non-empty UUID.
    /// </summary>
    [Fact]
    public void Generate_Should_Return_NonEmptyGuid()
    {
        // Act
        Guid guid = Uuid7MonotonicGenerator.Generate();

        // Assert
        Assert.NotEqual(Guid.Empty, guid);
    }

    /// <summary>
    /// Verifies that consecutive UUIDs are unique.
    /// </summary>
    [Fact]
    public void Generate_Should_Return_UniqueGuids()
    {
        // Act
        Guid guid1 = Uuid7MonotonicGenerator.Generate();
        Guid guid2 = Uuid7MonotonicGenerator.Generate();

        // Assert
        Assert.NotEqual(guid1, guid2);
    }

    /// <summary>
    /// Verifies that a large number of generated UUIDs are unique.
    /// </summary>
    [Fact]
    public void Generate_MultipleGuids_Should_AllBeUnique()
    {
        // Arrange
        const int count = 10_000;

        // Act
        List<Guid> guids = new(count);

        for (int i = 0; i < count; i++)
        {
            guids.Add(
                Uuid7MonotonicGenerator.Generate());
        }

        // Assert
        Assert.Equal(
            count,
            guids.Distinct().Count());
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
    /// Verifies that generated UUIDs contain
    /// the RFC 9562 variant.
    /// </summary>
    [Fact]
    public void Generate_Should_SetRfcVariant()
    {
        // Act
        Guid guid =
            Uuid7MonotonicGenerator.Generate();

        byte[] bytes = guid.ToByteArray();

        int variant = (bytes[8] >> 6) & 0b11;

        // Assert
        Assert.Equal(0b10, variant);
    }

    /// <summary>
    /// Verifies that UUID generation is thread-safe.
    /// </summary>
    [Fact]
    public async Task Generate_Should_BeThreadSafe()
    {
        // Arrange
        const int taskCount = 20;
        const int guidsPerTask = 500;

        ConcurrentBag<Guid> guids = [];

        // Act
        Task[] tasks =
            Enumerable.Range(0, taskCount)
                .Select(_ =>
                    Task.Run(() =>
                    {
                        for (int i = 0;
                             i < guidsPerTask;
                             i++)
                        {
                            guids.Add(
                                Uuid7MonotonicGenerator.Generate());
                        }
                    }))
                .ToArray();

        await Task.WhenAll(tasks);

        // Assert
        Assert.Equal(
            taskCount * guidsPerTask,
            guids.Count);

        Assert.Equal(
            guids.Count,
            guids.Distinct().Count());
    }

    /// <summary>
    /// Verifies that UUIDs generated sequentially
    /// are monotonically increasing.
    /// </summary>
    /// <remarks>
    /// This test validates the monotonic behaviour
    /// of the generator by comparing consecutive UUIDs.
    /// </remarks>
    [Fact]
    public void Generate_Should_Produce_MonotonicSequence()
    {
        // Arrange
        const int count = 100;

        List<Guid> guids = new(count);

        // Act
        for (int i = 0; i < count; i++)
        {
            guids.Add(
                Uuid7MonotonicGenerator.Generate());
        }

        // Assert
        for (int i = 1; i < guids.Count; i++)
        {
            Assert.NotEqual(
                guids[i - 1],
                guids[i]);
        }
    }

    /// <summary>
    /// Verifies that multiple consecutive UUIDs
    /// can be generated without throwing exceptions.
    /// </summary>
    [Fact]
    public void Generate_Repeatedly_Should_NotThrow()
    {
        // Act
        Exception? exception =
            Record.Exception(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    _ = Uuid7MonotonicGenerator.Generate();
                }
            });

        // Assert
        Assert.Null(exception);
    }

    /// <summary>
    /// Verifies that UUID generation succeeds under
    /// heavy concurrent load.
    /// </summary>
    [Fact]
    public async Task Generate_UnderHighConcurrency_Should_NotProduceDuplicates()
    {
        // Arrange
        const int taskCount = 50;
        const int iterations = 1000;

        ConcurrentBag<Guid> results = [];

        // Act
        await Parallel.ForEachAsync(
            Enumerable.Range(0, taskCount),
            async (_, _) =>
            {
                for (int i = 0; i < iterations; i++)
                {
                    results.Add(
                        Uuid7MonotonicGenerator.Generate());
                }

                await Task.CompletedTask;
            });

        // Assert
        Assert.Equal(
            taskCount * iterations,
            results.Count);

        Assert.Equal(
            results.Count,
            results.Distinct().Count());
    }
}