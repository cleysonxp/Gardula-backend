using Gardula.Application.Authentication.DTOs;
using Gardula.Application.Authentication.Exceptions;
using Gardula.Application.Authentication.Services;
using Gardula.Domain.Entities.Authentication;
using Gardula.UnitTests.Authentication.Fakes;

namespace Gardula.UnitTests.Authentication;

public class LoginServiceTests
{
    private readonly FakeUserRepository _userRepository;
    private readonly FakePasswordHasher _passwordHasher;
    private readonly FakeTokenService _tokenService;
    private readonly FakeRefreshTokenHasher _refreshTokenHasher;
    private readonly FakeRefreshTokenRepository _refreshTokenRepository;
    private readonly LoginService _loginService;

    public LoginServiceTests()
    {
        _userRepository = new FakeUserRepository();
        _passwordHasher = new FakePasswordHasher();
        _tokenService = new FakeTokenService();
        _refreshTokenHasher = new FakeRefreshTokenHasher();
        _refreshTokenRepository = new FakeRefreshTokenRepository();

        _loginService = new LoginService(
            _userRepository,
            _passwordHasher,
            _tokenService,
            _refreshTokenHasher,
            _refreshTokenRepository);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLogin_WhenCredentialsAreValid()
    {
        // Arrange
        var user = CreateUser();

        await _userRepository.AddAsync(user);

        var request = new LoginRequest
        {
            Email = "cleyson@email.com",
            Password = "Gardula@123"
        };

        // Act
        var response = await _loginService.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);

        Assert.Equal(
            "fake-access-token",
            response.AccessToken);

        Assert.Equal(
            "fake-refresh-token",
            response.RefreshToken);

        Assert.Single(_refreshTokenRepository.RefreshTokens);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowInvalidCredentials_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "unknown@email.com",
            Password = "Gardula@123"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(
            () => _loginService.ExecuteAsync(request));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowInvalidCredentials_WhenPasswordIsIncorrect()
    {
        // Arrange
        var user = CreateUser();

        await _userRepository.AddAsync(user);

        var request = new LoginRequest
        {
            Email = "cleyson@email.com",
            Password = "Wrong@123"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(
            () => _loginService.ExecuteAsync(request));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowInvalidCredentials_WhenUserIsInactive()
    {
        // Arrange
        var user = CreateUser();

        user.Deactivate();

        await _userRepository.AddAsync(user);

        var request = new LoginRequest
        {
            Email = "cleyson@email.com",
            Password = "Gardula@123"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(
            () => _loginService.ExecuteAsync(request));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNormalizeEmail()
    {
        // Arrange
        var user = CreateUser();

        await _userRepository.AddAsync(user);

        var request = new LoginRequest
        {
            Email = "  CLEYSON@EMAIL.COM  ",
            Password = "Gardula@123"
        };

        // Act
        var response = await _loginService.ExecuteAsync(request);

        // Assert
        Assert.Equal(
            "fake-access-token",
            response.AccessToken);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldStoreRefreshTokenHash()
    {
        // Arrange
        var user = CreateUser();

        await _userRepository.AddAsync(user);

        var request = new LoginRequest
        {
            Email = "cleyson@email.com",
            Password = "Gardula@123"
        };

        // Act
        await _loginService.ExecuteAsync(request);

        // Assert
        var refreshToken =
            _refreshTokenRepository.RefreshTokens.Single();

        Assert.Equal(
            "HASHED:fake-refresh-token",
            refreshToken.TokenHash);

        Assert.Equal(
            "fake-refresh-token",
            _refreshTokenHasher.TokenReceived);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldRevokeExistingRefreshToken()
    {
        // Arrange
        var user = CreateUser();

        await _userRepository.AddAsync(user);

        var firstRequest = new LoginRequest
        {
            Email = "cleyson@email.com",
            Password = "Gardula@123"
        };

        await _loginService.ExecuteAsync(firstRequest);

        var oldToken =
            _refreshTokenRepository.RefreshTokens.Single();

        // Act
        var secondRequest = new LoginRequest
        {
            Email = "cleyson@email.com",
            Password = "Gardula@123"
        };

        await _loginService.ExecuteAsync(secondRequest);

        // Assert
        Assert.NotNull(oldToken.RevokedAt);
    }

    private static User CreateUser()
    {
        var user = new User(
            "Cleyson",
            "cleyson@email.com",
            "HASHED:Gardula@123");

        typeof(User)
            .GetProperty(nameof(User.Id))!
            .SetValue(user, 1);

        return user;
    }
}