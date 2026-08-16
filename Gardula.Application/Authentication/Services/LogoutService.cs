using Gardula.Application.Authentication.DTOs;
using Gardula.Application.Authentication.Exceptions;

namespace Gardula.Application.Authentication.Services;

public class LogoutService : ILogoutService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRefreshTokenHasher _refreshTokenHasher;

    public LogoutService(
        IRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenHasher refreshTokenHasher)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokenHasher = refreshTokenHasher;
    }

    public async Task ExecuteAsync(
        LogoutRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            throw new InvalidRefreshTokenException();

        var tokenHash =
            _refreshTokenHasher.Hash(request.RefreshToken);

        var refreshToken =
            await _refreshTokenRepository.GetByTokenHashAsync(
                tokenHash,
                cancellationToken);

        if (refreshToken is null)
            throw new InvalidRefreshTokenException();

        if (!refreshToken.IsActive())
            throw new InvalidRefreshTokenException();

        refreshToken.Revoke();

        await _refreshTokenRepository.UpdateAsync(
            refreshToken,
            cancellationToken);
    }
}