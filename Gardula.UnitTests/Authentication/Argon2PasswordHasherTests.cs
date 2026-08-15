using Gardula.Infrastructure.Authentication.Services;

namespace Gardula.UnitTests.Authentication;

public class Argon2PasswordHasherTests
{
    private readonly Argon2PasswordHasher _passwordHasher;

    public Argon2PasswordHasherTests()
    {
        _passwordHasher = new Argon2PasswordHasher();
    }

    [Fact]
    public void Hash_ShouldReturnDifferentHashes_WhenCalledWithSamePassword()
    {
        // Arrange
        const string password = "Gardula@123";

        // Act
        var firstHash = _passwordHasher.Hash(password);
        var secondHash = _passwordHasher.Hash(password);

        // Assert
        Assert.NotEqual(firstHash, secondHash);
    }

    [Fact]
    public void Verify_ShouldReturnTrue_WhenPasswordIsCorrect()
    {
        // Arrange
        const string password = "Gardula@123";

        var passwordHash = _passwordHasher.Hash(password);

        // Act
        var result = _passwordHasher.Verify(password, passwordHash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Verify_ShouldReturnFalse_WhenPasswordIsIncorrect()
    {
        // Arrange
        const string password = "Gardula@123";
        const string incorrectPassword = "Gardula@456";

        var passwordHash = _passwordHasher.Hash(password);

        // Act
        var result = _passwordHasher.Verify(
            incorrectPassword,
            passwordHash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Hash_ShouldThrowArgumentException_WhenPasswordIsEmpty()
    {
        // Arrange
        const string password = "";

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _passwordHasher.Hash(password));
    }

    [Fact]
    public void Hash_ShouldThrowArgumentException_WhenPasswordIsWhiteSpace()
    {
        // Arrange
        const string password = "   ";

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _passwordHasher.Hash(password));
    }

    [Fact]
    public void Verify_ShouldThrowArgumentException_WhenPasswordIsEmpty()
    {
        // Arrange
        const string password = "";

        var passwordHash = _passwordHasher.Hash("Gardula@123");

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _passwordHasher.Verify(password, passwordHash));
    }

    [Fact]
    public void Verify_ShouldReturnFalse_WhenPasswordHashIsInvalid()
    {
        // Arrange
        const string password = "Gardula@123";
        const string invalidHash = "hash-invalido";

        // Act
        var result = _passwordHasher.Verify(password, invalidHash);

        // Assert
        Assert.False(result);
    }
}