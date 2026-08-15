using Gardula.Application.Authentication.DTOs;
using Gardula.Application.Authentication.Exceptions;
using Gardula.Application.Authentication.Services;
using Gardula.UnitTests.Authentication.Fakes;

namespace Gardula.UnitTests.Authentication;

public class RegisterServiceTests
{
    private readonly FakeUserRepository _userRepository;
    private readonly FakePasswordHasher _passwordHasher;
    private readonly RegisterService _registerService;

    public RegisterServiceTests()
    {
        _userRepository = new FakeUserRepository();
        _passwordHasher = new FakePasswordHasher();

        _registerService = new RegisterService(
            _userRepository,
            _passwordHasher);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCreateUser_WhenRequestIsValid()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Name = "Cleyson",
            Email = "cleyson@email.com",
            Password = "Gardula@123"
        };

        // Act
        var response = await _registerService.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("Cleyson", response.Name);
        Assert.Equal("cleyson@email.com", response.Email);

        Assert.Single(_userRepository.Users);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNormalizeEmail_WhenEmailContainsUppercaseOrSpaces()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Name = "Cleyson",
            Email = "  CLEYSON@EMAIL.COM  ",
            Password = "Gardula@123"
        };

        // Act
        var response = await _registerService.ExecuteAsync(request);

        // Assert
        Assert.Equal(
            "cleyson@email.com",
            response.Email);

        Assert.Equal(
            "cleyson@email.com",
            _userRepository.Users.Single().Email);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowInvalidPasswordException_WhenPasswordIsInvalid()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Name = "Cleyson",
            Email = "cleyson@email.com",
            Password = "gardula123"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidPasswordException>(
            () => _registerService.ExecuteAsync(request));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowEmailAlreadyExistsException_WhenEmailAlreadyExists()
    {
        // Arrange
        var existingRequest = new RegisterRequest
        {
            Name = "Cleyson",
            Email = "cleyson@email.com",
            Password = "Gardula@123"
        };

        await _registerService.ExecuteAsync(existingRequest);

        var duplicateRequest = new RegisterRequest
        {
            Name = "Outro Usuário",
            Email = "cleyson@email.com",
            Password = "Outra@123"
        };

        // Act & Assert
        await Assert.ThrowsAsync<EmailAlreadyExistsException>(
            () => _registerService.ExecuteAsync(duplicateRequest));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldHashPassword_BeforeCreatingUser()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Name = "Cleyson",
            Email = "cleyson@email.com",
            Password = "Gardula@123"
        };

        // Act
        await _registerService.ExecuteAsync(request);

        // Assert
        var user = _userRepository.Users.Single();

        Assert.Equal(
            "HASHED:Gardula@123",
            user.PasswordHash);

        Assert.Equal(
            "Gardula@123",
            _passwordHasher.PasswordReceived);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNotStorePlainPassword()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Name = "Cleyson",
            Email = "cleyson@email.com",
            Password = "Gardula@123"
        };

        // Act
        await _registerService.ExecuteAsync(request);

        // Assert
        var user = _userRepository.Users.Single();

        Assert.NotEqual(
            request.Password,
            user.PasswordHash);
    }
}