using Dixor.Identity.UUID7.Internal;
using Xunit;

namespace Dixor.Identity.Tests.UUID7;

/// <summary>
/// Contains unit tests for
/// <see cref="Uuid7Validator"/>.
/// </summary>
public sealed class Uuid7ValidatorTests
{
    /// <summary>
    /// Verifies that UUIDv7 values are recognized.
    /// </summary>
    [Fact]
    public void IsUuid7_Should_ReturnTrue_ForUuid7()
    {
        // Arrange
        Guid guid = Uuid7Generator.Generate();

        // Act
        bool result =
            Uuid7Validator.IsUuid7(guid);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Verifies that randomly generated GUIDs are not
    /// recognized as UUIDv7 values.
    /// </summary>
    [Fact]
    public void IsUuid7_Should_ReturnFalse_ForNonUuid7()
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act
        bool result =
            Uuid7Validator.IsUuid7(guid);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Verifies that Guid.Empty is not recognized
    /// as a UUIDv7 value.
    /// </summary>
    [Fact]
    public void IsUuid7_Should_ReturnFalse_ForEmptyGuid()
    {
        // Arrange
        Guid guid = Guid.Empty;

        // Act
        bool result =
            Uuid7Validator.IsUuid7(guid);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Verifies that a valid UUIDv7 string
    /// is recognized.
    /// </summary>
    [Fact]
    public void IsValid_Should_ReturnTrue_ForValidUuid7String()
    {
        // Arrange
        Guid guid = Uuid7Generator.Generate();

        string input = guid.ToString();

        // Act
        bool result =
            Uuid7Validator.IsValid(input);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Verifies that UUIDv7 values embedded within
    /// larger text are recognized.
    /// </summary>
    [Fact]
    public void IsValid_Should_ReturnTrue_ForEmbeddedUuid7()
    {
        // Arrange
        Guid guid = Uuid7Generator.Generate();

        string input =
            $"Request Id: {guid}";

        // Act
        bool result =
            Uuid7Validator.IsValid(input);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Verifies that non-UUIDv7 strings are rejected.
    /// </summary>
    [Fact]
    public void IsValid_Should_ReturnFalse_ForNonUuid7String()
    {
        // Arrange
        string input = Guid.NewGuid().ToString();

        // Act
        bool result =
            Uuid7Validator.IsValid(input);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Verifies that malformed input is rejected.
    /// </summary>
    [Fact]
    public void IsValid_Should_ReturnFalse_ForMalformedInput()
    {
        // Arrange
        const string input =
            "This is not a UUID.";

        // Act
        bool result =
            Uuid7Validator.IsValid(input);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Verifies that an empty string is rejected.
    /// </summary>
    [Fact]
    public void IsValid_Should_ReturnFalse_ForEmptyString()
    {
        // Arrange
        const string input = "";

        // Act
        bool result =
            Uuid7Validator.IsValid(input);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Verifies that whitespace input is rejected.
    /// </summary>
    [Fact]
    public void IsValid_Should_ReturnFalse_ForWhitespace()
    {
        // Arrange
        const string input = "   ";

        // Act
        bool result =
            Uuid7Validator.IsValid(input);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Verifies that multiple generated UUIDv7 values
    /// are all recognized correctly.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    public void IsUuid7_Should_ReturnTrue_ForMultipleGeneratedValues(
        int count)
    {
        // Arrange
        IEnumerable<Guid> guids =
            Enumerable.Range(0, count)
                .Select(_ => Uuid7Generator.Generate());

        // Act & Assert
        foreach (Guid guid in guids)
        {
            Assert.True(
                Uuid7Validator.IsUuid7(guid));
        }
    }

    /// <summary>
    /// Verifies that uppercase UUIDv7 strings
    /// are supported.
    /// </summary>
    [Fact]
    public void IsValid_Should_SupportUppercaseUuid()
    {
        // Arrange
        string input =
            Uuid7Generator.Generate()
                .ToString()
                .ToUpperInvariant();

        // Act
        bool result =
            Uuid7Validator.IsValid(input);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Verifies that lowercase UUIDv7 strings
    /// are supported.
    /// </summary>
    [Fact]
    public void IsValid_Should_SupportLowercaseUuid()
    {
        // Arrange
        string input =
            Uuid7Generator.Generate()
                .ToString()
                .ToLowerInvariant();

        // Act
        bool result =
            Uuid7Validator.IsValid(input);

        // Assert
        Assert.True(result);
    }
}