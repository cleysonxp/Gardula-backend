using Gardula.Application.Authentication.DTOs;
using Gardula.Application.Authentication.Exceptions;
using Gardula.Application.Authentication.Services;
using Gardula.Domain.Entities.Authentication;
using Gardula.UnitTests.Authentication.Fakes;

namespace Gardula.UnitTests.Authentication;

public class LogoutServiceTests
{
    private readonly FakeRefreshTokenHasher _refreshTokenHasher;
    private readonly FakeRefreshTokenRepository _refreshTokenRepository;
    private readonly LogoutService _logoutService;

    public LogoutServiceTests()
    {
        _refreshTokenHasher = new FakeRefreshTokenHasher();
        _refreshTokenRepository = new FakeRefreshTokenRepository();

        _logoutService = new LogoutService(
            _refreshTokenRepository,
            _refreshTokenHasher);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldRevokeRefreshToken_WhenTokenIsValid()
    {
        // Arrange
        var refreshToken = CreateRefreshToken();

        await _refreshTokenRepository.AddAsync(refreshToken);

        var request = new LogoutRequest
        {
            RefreshToken = "fake-refresh-token"
        };

        // Act
        await _logoutService.ExecuteAsync(request);

        // Assert
        Assert.NotNull(refreshToken.RevokedAt);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrow_WhenRefreshTokenIsEmpty()
    {
        // Arrange
        var request = new LogoutRequest
        {
            RefreshToken = string.Empty
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidRefreshTokenException>(
            () => _logoutService.ExecuteAsync(request));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrow_WhenRefreshTokenDoesNotExist()
    {
        // Arrange
        var request = new LogoutRequest
        {
            RefreshToken = "unknown-token"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidRefreshTokenException>(
            () => _logoutService.ExecuteAsync(request));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrow_WhenRefreshTokenIsAlreadyRevoked()
    {
        // Arrange
        var refreshToken = CreateRefreshToken();

        refreshToken.Revoke();

        await _refreshTokenRepository.AddAsync(refreshToken);

        var request = new LogoutRequest
        {
            RefreshToken = "fake-refresh-token"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidRefreshTokenException>(
            () => _logoutService.ExecuteAsync(request));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrow_WhenRefreshTokenIsExpired()
    {
        // Arrange
        var refreshToken = CreateRefreshToken();

        typeof(RefreshToken)
            .GetProperty(nameof(RefreshToken.ExpiresAt))!
            .SetValue(
                refreshToken,
                DateTimeOffset.UtcNow.AddMinutes(-1));

        await _refreshTokenRepository.AddAsync(refreshToken);

        var request = new LogoutRequest
        {
            RefreshToken = "fake-refresh-token"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidRefreshTokenException>(
            () => _logoutService.ExecuteAsync(request));
    }

    private static RefreshToken CreateRefreshToken()
    {
        return new RefreshToken(
            1,
            "HASHED:fake-refresh-token",
            DateTimeOffset.UtcNow.AddDays(7));
    }
}