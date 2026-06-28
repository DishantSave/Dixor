using Dixor.Identity.UUID7.Internal;
using Xunit;

namespace Dixor.Identity.Tests.UUID7;

/// <summary>
/// Contains unit tests for <see cref="Uuid7Parser"/>.
/// </summary>
public sealed class Uuid7ParserTests
{
    /// <summary>
    /// Verifies that Parse returns the UUID
    /// when the input contains a valid UUID.
    /// </summary>
    [Fact]
    public void Parse_Should_ReturnGuid_When_InputContainsGuid()
    {
        // Arrange
        Guid expected = Guid.NewGuid();

        string input =
            $"Prefix-{expected}-Suffix";

        // Act
        Guid result =
            Uuid7Parser.Parse(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Verifies that Parse throws when
    /// no UUID is present.
    /// </summary>
    [Fact]
    public void Parse_Should_ThrowFormatException_When_NoGuidExists()
    {
        // Arrange
        string input = "No UUID present.";

        // Act & Assert
        Assert.Throws<FormatException>(
            () => Uuid7Parser.Parse(input));
    }

    /// <summary>
    /// Verifies that TryParse succeeds
    /// for a valid UUID string.
    /// </summary>
    [Fact]
    public void TryParse_Should_ReturnTrue_ForValidGuid()
    {
        // Arrange
        Guid expected = Guid.NewGuid();

        // Act
        bool result =
            Uuid7Parser.TryParse(
                expected.ToString(),
                out Guid actual);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Verifies that TryParse succeeds
    /// when the UUID is embedded in text.
    /// </summary>
    [Fact]
    public void TryParse_Should_ReturnTrue_When_GuidEmbeddedInText()
    {
        // Arrange
        Guid expected = Guid.NewGuid();

        string input =
            $"Order Id: {expected}";

        // Act
        bool result =
            Uuid7Parser.TryParse(
                input,
                out Guid actual);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Verifies that TryParse returns false
    /// when no UUID exists.
    /// </summary>
    [Fact]
    public void TryParse_Should_ReturnFalse_When_NoGuidExists()
    {
        // Arrange
        string input = "Invalid input";

        // Act
        bool result =
            Uuid7Parser.TryParse(
                input,
                out Guid actual);

        // Assert
        Assert.False(result);
        Assert.Equal(Guid.Empty, actual);
    }

    /// <summary>
    /// Verifies that TryParse returns false
    /// for an empty string.
    /// </summary>
    [Fact]
    public void TryParse_Should_ReturnFalse_ForEmptyString()
    {
        // Arrange
        string input = string.Empty;

        // Act
        bool result =
            Uuid7Parser.TryParse(
                input,
                out Guid actual);

        // Assert
        Assert.False(result);
        Assert.Equal(Guid.Empty, actual);
    }

    /// <summary>
    /// Verifies that TryParse returns false
    /// for whitespace input.
    /// </summary>
    [Fact]
    public void TryParse_Should_ReturnFalse_ForWhitespace()
    {
        // Arrange
        string input = "   ";

        // Act
        bool result =
            Uuid7Parser.TryParse(
                input,
                out Guid actual);

        // Assert
        Assert.False(result);
        Assert.Equal(Guid.Empty, actual);
    }

    /// <summary>
    /// Verifies that the first UUID is returned
    /// when multiple UUIDs exist.
    /// </summary>
    [Fact]
    public void Parse_Should_ReturnFirstGuid_When_MultipleGuidsExist()
    {
        // Arrange
        Guid first = Guid.NewGuid();
        Guid second = Guid.NewGuid();

        string input =
            $"{first} some text {second}";

        // Act
        Guid result =
            Uuid7Parser.Parse(input);

        // Assert
        Assert.Equal(first, result);
    }

    /// <summary>
    /// Verifies that uppercase UUIDs
    /// are parsed successfully.
    /// </summary>
    [Fact]
    public void TryParse_Should_SupportUppercaseGuid()
    {
        // Arrange
        Guid expected = Guid.NewGuid();

        string input =
            expected.ToString().ToUpperInvariant();

        // Act
        bool result =
            Uuid7Parser.TryParse(
                input,
                out Guid actual);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Verifies that lowercase UUIDs
    /// are parsed successfully.
    /// </summary>
    [Fact]
    public void TryParse_Should_SupportLowercaseGuid()
    {
        // Arrange
        Guid expected = Guid.NewGuid();

        string input =
            expected.ToString().ToLowerInvariant();

        // Act
        bool result =
            Uuid7Parser.TryParse(
                input,
                out Guid actual);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Verifies that malformed UUID text
    /// is rejected.
    /// </summary>
    [Fact]
    public void TryParse_Should_ReturnFalse_ForMalformedGuid()
    {
        // Arrange
        string input =
            "12345678-1234-1234-1234-invalid";

        // Act
        bool result =
            Uuid7Parser.TryParse(
                input,
                out Guid actual);

        // Assert
        Assert.False(result);
        Assert.Equal(Guid.Empty, actual);
    }
}