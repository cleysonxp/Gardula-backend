using Gardula.Application.Authentication.Validation;

namespace Gardula.UnitTests.Authentication;

public class PasswordPolicyTests
{
    [Theory]
    [InlineData("Gardula@123")]
    [InlineData("Senha@123")]
    [InlineData("Teste#2026")]
    public void IsValid_ShouldReturnTrue_WhenPasswordMeetsRequirements(
        string password)
    {
        // Act
        var result = PasswordPolicy.IsValid(password);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("Gardu@1")]
    [InlineData("Test@12")]
    public void IsValid_ShouldReturnFalse_WhenPasswordHasLessThanEightCharacters(
        string password)
    {
        // Act
        var result = PasswordPolicy.IsValid(password);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenPasswordHasNoUppercaseLetter()
    {
        // Arrange
        const string password = "gardula@123";

        // Act
        var result = PasswordPolicy.IsValid(password);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenPasswordHasNoLowercaseLetter()
    {
        // Arrange
        const string password = "GARDULA@123";

        // Act
        var result = PasswordPolicy.IsValid(password);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenPasswordHasNoSpecialCharacter()
    {
        // Arrange
        const string password = "Gardula123";

        // Act
        var result = PasswordPolicy.IsValid(password);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("       ")]
    public void IsValid_ShouldReturnFalse_WhenPasswordIsEmptyOrWhiteSpace(
        string password)
    {
        // Act
        var result = PasswordPolicy.IsValid(password);

        // Assert
        Assert.False(result);
    }
}