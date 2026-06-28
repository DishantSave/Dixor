using Dixor.Identity.UUID7.Internal;
using Xunit;

namespace Dixor.Identity.Tests.UUID7;

/// <summary>
/// Contains unit tests for <see cref="Uuid7Formatter"/>.
/// </summary>
public sealed class Uuid7FormatterTests
{
    /// <summary>
    /// Verifies that formatting without a prefix or suffix
    /// returns the GUID string representation.
    /// </summary>
    [Fact]
    public void Format_WithoutPrefixOrSuffix_ShouldReturnGuidString()
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act
        string result = Uuid7Formatter.Format(guid);

        // Assert
        Assert.Equal(guid.ToString(), result);
    }

    /// <summary>
    /// Verifies that a prefix is correctly prepended.
    /// </summary>
    [Fact]
    public void Format_WithPrefix_ShouldPrependPrefix()
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act
        string result =
            Uuid7Formatter.Format(guid, prefix: "DS");

        // Assert
        Assert.StartsWith("DS-", result);
    }

    /// <summary>
    /// Verifies that a suffix is correctly appended.
    /// </summary>
    [Fact]
    public void Format_WithSuffix_ShouldAppendSuffix()
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act
        string result =
            Uuid7Formatter.Format(guid, suffix: "PSA");

        // Assert
        Assert.EndsWith("-PSA", result);
    }

    /// <summary>
    /// Verifies that both prefix and suffix are correctly applied.
    /// </summary>
    [Fact]
    public void Format_WithPrefixAndSuffix_ShouldFormatCorrectly()
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act
        string result =
            Uuid7Formatter.Format(
                guid,
                prefix: "DS",
                suffix: "PSA",
                separator: "_");

        // Assert
        Assert.StartsWith("DS_", result);
        Assert.EndsWith("_PSA", result);
    }

    /// <summary>
    /// Verifies that whitespace prefixes are ignored.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Format_WithWhitespacePrefix_ShouldIgnorePrefix(
        string prefix)
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act
        string result =
            Uuid7Formatter.Format(guid, prefix: prefix);

        // Assert
        Assert.Equal(guid.ToString(), result);
    }

    /// <summary>
    /// Verifies that an exception is thrown when
    /// the prefix exceeds the supported length.
    /// </summary>
    [Fact]
    public void Format_WithLongPrefix_ShouldThrow()
    {
        // Arrange
        Guid guid = Guid.NewGuid();
        string prefix =
            new('A', Uuid7Formatter.MaxPrefixLength + 1);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(
            () => Uuid7Formatter.Format(guid, prefix: prefix));
    }

    /// <summary>
    /// Verifies that an exception is thrown when
    /// the suffix exceeds the supported length.
    /// </summary>
    [Fact]
    public void Format_WithLongSuffix_ShouldThrow()
    {
        // Arrange
        Guid guid = Guid.NewGuid();
        string suffix =
            new('A', Uuid7Formatter.MaxSuffixLength + 1);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(
            () => Uuid7Formatter.Format(guid, suffix: suffix));
    }

    /// <summary>
    /// Verifies that an invalid separator throws an exception.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("AB")]
    [InlineData("A")]
    [InlineData("1")]
    public void Format_WithInvalidSeparator_ShouldThrow(
        string separator)
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => Uuid7Formatter.Format(
                guid,
                separator: separator));
    }

    /// <summary>
    /// Verifies that all supported separators are accepted.
    /// </summary>
    [Theory]
    [InlineData("-")]
    [InlineData(":")]
    [InlineData("~")]
    [InlineData("|")]
    [InlineData("{")]
    [InlineData("}")]
    [InlineData("[")]
    [InlineData("]")]
    [InlineData("<")]
    [InlineData(">")]
    [InlineData("/")]
    [InlineData("\\")]
    [InlineData("_")]
    public void Format_WithSupportedSeparator_ShouldSucceed(
        string separator)
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act
        string result =
            Uuid7Formatter.Format(
                guid,
                prefix: "DS",
                separator: separator);

        // Assert
        Assert.Contains(separator, result);
    }
}