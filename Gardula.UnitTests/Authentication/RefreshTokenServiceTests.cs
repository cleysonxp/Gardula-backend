using Gardula.Application.Authentication.DTOs;
using Gardula.Application.Authentication.Exceptions;
using Gardula.Application.Authentication.Services;
using Gardula.Domain.Entities.Authentication;
using Gardula.UnitTests.Authentication.Fakes;

namespace Gardula.UnitTests.Authentication;

public class RefreshTokenServiceTests
{
    private readonly FakeUserRepository _userRepository;
    private readonly FakeTokenService _tokenService;
    private readonly FakeRefreshTokenHasher _refreshTokenHasher;
    private readonly FakeRefreshTokenRepository _refreshTokenRepository;
    private readonly RefreshTokenService _refreshTokenService;

    public RefreshTokenServiceTests()
    {
        _userRepository = new FakeUserRepository();
        _tokenService = new FakeTokenService();
        _refreshTokenHasher = new FakeRefreshTokenHasher();
        _refreshTokenRepository = new FakeRefreshTokenRepository();

        _refreshTokenService = new RefreshTokenService(
            _refreshTokenRepository,
            _refreshTokenHasher,
            _userRepository,
            _tokenService);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldRefreshTokens_WhenRefreshTokenIsValid()
    {
        // Arrange
        var user = CreateUser();
        await _userRepository.AddAsync(user);

        var refreshToken = CreateRefreshToken(user);

        await _refreshTokenRepository.AddAsync(refreshToken);

        var request = new RefreshTokenRequest
        {
            RefreshToken = "fake-refresh-token"
        };

        // Act
        var response =
            await _refreshTokenService.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);

        Assert.Equal(
            "fake-access-token",
            response.AccessToken);

        Assert.Equal(
            "fake-refresh-token",
            response.RefreshToken);

        Assert.NotNull(refreshToken.RevokedAt);

        Assert.Equal(
            2,
            _refreshTokenRepository.RefreshTokens.Count);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrow_WhenRefreshTokenDoesNotExist()
    {
        // Arrange
        var request = new RefreshTokenRequest
        {
            RefreshToken = "unknown-token"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidRefreshTokenException>(
            () => _refreshTokenService.ExecuteAsync(request));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrow_WhenRefreshTokenIsEmpty()
    {
        // Arrange
        var request = new RefreshTokenRequest
        {
            RefreshToken = string.Empty
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidRefreshTokenException>(
            () => _refreshTokenService.ExecuteAsync(request));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrow_WhenRefreshTokenIsRevoked()
    {
        // Arrange
        var user = CreateUser();
        await _userRepository.AddAsync(user);

        var refreshToken = CreateRefreshToken(user);

        refreshToken.Revoke();

        await _refreshTokenRepository.AddAsync(refreshToken);

        var request = new RefreshTokenRequest
        {
            RefreshToken = "fake-refresh-token"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidRefreshTokenException>(
            () => _refreshTokenService.ExecuteAsync(request));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrow_WhenRefreshTokenIsExpired()
    {
        // Arrange
        var user = CreateUser();

        await _userRepository.AddAsync(user);

        var expiredToken = new RefreshToken(
            user.Id,
            "HASHED:fake-refresh-token",
            DateTimeOffset.UtcNow.AddDays(7));

        typeof(RefreshToken)
            .GetProperty(nameof(RefreshToken.ExpiresAt))!
            .SetValue(
                expiredToken,
                DateTimeOffset.UtcNow.AddMinutes(-1));

        await _refreshTokenRepository.AddAsync(expiredToken);

        var request = new RefreshTokenRequest
        {
            RefreshToken = "fake-refresh-token"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidRefreshTokenException>(
            () => _refreshTokenService.ExecuteAsync(request));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrow_WhenUserDoesNotExist()
    {
        // Arrange
        var refreshToken = new RefreshToken(
            999,
            "HASHED:fake-refresh-token",
            DateTimeOffset.UtcNow.AddDays(7));

        await _refreshTokenRepository.AddAsync(refreshToken);

        var request = new RefreshTokenRequest
        {
            RefreshToken = "fake-refresh-token"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidRefreshTokenException>(
            () => _refreshTokenService.ExecuteAsync(request));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrow_WhenUserIsInactive()
    {
        // Arrange
        var user = CreateUser();

        user.Deactivate();

        await _userRepository.AddAsync(user);

        var refreshToken = CreateRefreshToken(user);

        await _refreshTokenRepository.AddAsync(refreshToken);

        var request = new RefreshTokenRequest
        {
            RefreshToken = "fake-refresh-token"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidRefreshTokenException>(
            () => _refreshTokenService.ExecuteAsync(request));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldStoreNewRefreshTokenAsHash()
    {
        // Arrange
        var user = CreateUser();
        await _userRepository.AddAsync(user);

        var refreshToken = CreateRefreshToken(user);

        await _refreshTokenRepository.AddAsync(refreshToken);

        var request = new RefreshTokenRequest
        {
            RefreshToken = "fake-refresh-token"
        };

        // Act
        await _refreshTokenService.ExecuteAsync(request);

        // Assert
        var newToken =
            _refreshTokenRepository.RefreshTokens
                .Last();

        Assert.Equal(
            "HASHED:fake-refresh-token",
            newToken.TokenHash);
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

    private static RefreshToken CreateRefreshToken(User user)
    {
        return new RefreshToken(
            user.Id,
            "HASHED:fake-refresh-token",
            DateTimeOffset.UtcNow.AddDays(7));
    }
}