using Dixor.Identity.UUID7.Internal;
using Xunit;

namespace Dixor.Identity.Tests.UUID7;

/// <summary>
/// Contains unit tests for <see cref="GuidByteConverter"/>.
/// </summary>
public sealed class GuidByteConverterTests
{
    /// <summary>
    /// Verifies that a GUID can be converted to bytes and back
    /// without losing information.
    /// </summary>
    [Fact]
    public void ToBytes_ThenToGuid_Should_Return_OriginalGuid()
    {
        // Arrange
        Guid expected = Guid.NewGuid();
        Span<byte> bytes = stackalloc byte[16];

        // Act
        GuidByteConverter.ToBytes(expected, bytes);
        Guid actual = GuidByteConverter.ToGuid(bytes);

        // Assert
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Verifies that converting <see cref="Guid.Empty"/>
    /// to bytes and back returns <see cref="Guid.Empty"/>.
    /// </summary>
    [Fact]
    public void ToGuid_With_EmptyGuidBytes_Should_Return_EmptyGuid()
    {
        // Arrange
        Guid expected = Guid.Empty;
        Span<byte> bytes = stackalloc byte[16];

        GuidByteConverter.ToBytes(expected, bytes);

        // Act
        Guid actual = GuidByteConverter.ToGuid(bytes);

        // Assert
        Assert.Equal(Guid.Empty, actual);
    }

    /// <summary>
    /// Verifies that <see cref="GuidByteConverter.ToBytes"/>
    /// produces the same byte sequence as
    /// <see cref="Guid.TryWriteBytes(Span{byte})"/>.
    /// </summary>
    [Fact]
    public void ToBytes_Should_Match_GuidTryWriteBytes()
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        Span<byte> expected = stackalloc byte[16];
        Span<byte> actual = stackalloc byte[16];

        guid.TryWriteBytes(expected);

        // Act
        GuidByteConverter.ToBytes(guid, actual);

        // Assert
        Assert.True(
            expected.SequenceEqual(actual),
            "The byte sequence produced by GuidByteConverter did not match Guid.TryWriteBytes.");
    }

    /// <summary>
    /// Verifies that <see cref="GuidByteConverter.ToGuid"/>
    /// correctly reconstructs a known GUID from its byte representation.
    /// </summary>
    [Fact]
    public void ToGuid_With_KnownBytes_Should_Return_ExpectedGuid()
    {
        // Arrange
        Guid expected =
            Guid.Parse("01234567-89AB-CDEF-0123-456789ABCDEF");

        Span<byte> bytes = stackalloc byte[16];
        expected.TryWriteBytes(bytes);

        // Act
        Guid actual = GuidByteConverter.ToGuid(bytes);

        // Assert
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Verifies that <see cref="GuidByteConverter.ToGuid"/>
    /// throws an exception when the source span does not
    /// contain exactly sixteen bytes.
    /// </summary>
    [Fact]
    public void ToGuid_With_InvalidByteCount_Should_Throw_ArgumentException()
    {
        // Arrange
        byte[] bytes = new byte[15];

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => GuidByteConverter.ToGuid(bytes));
    }

    /// <summary>
    /// Verifies that <see cref="GuidByteConverter.ToBytes"/>
    /// throws an exception when the destination span is too small.
    /// </summary>
    [Fact]
    public void ToBytes_With_DestinationSmallerThan16Bytes_Should_Throw_ArgumentException()
    {
        // Arrange
        Guid guid = Guid.NewGuid();
        byte[] destination = new byte[15];

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => GuidByteConverter.ToBytes(guid, destination));
    }

    /// <summary>
    /// Verifies that the converter can successfully process
    /// multiple GUID values without corruption.
    /// </summary>
    [Fact]
    public void ToBytes_ThenToGuid_For_MultipleGuids_Should_PreserveValues()
    {
        // Arrange
        Guid[] guids =
        [
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid()
        ];

        Span<byte> bytes = stackalloc byte[16];

        foreach (Guid expected in guids)
        {
            // Act
            GuidByteConverter.ToBytes(expected, bytes);
            Guid actual = GuidByteConverter.ToGuid(bytes);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}