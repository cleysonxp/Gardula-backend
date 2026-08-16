using Gardula.Application.Authentication.DTOs;
using Gardula.Application.Authentication.Exceptions;
using Gardula.Domain.Entities.Authentication;

namespace Gardula.Application.Authentication.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRefreshTokenHasher _refreshTokenHasher;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public RefreshTokenService(
        IRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenHasher refreshTokenHasher,
        IUserRepository userRepository,
        ITokenService tokenService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokenHasher = refreshTokenHasher;
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> ExecuteAsync(
        RefreshTokenRequest request,
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

        var user =
            await _userRepository.GetByIdAsync(
                refreshToken.UserId,
                cancellationToken);

        if (user is null || !user.IsActive)
            throw new InvalidRefreshTokenException();

        refreshToken.Revoke();

        await _refreshTokenRepository.UpdateAsync(
            refreshToken,
            cancellationToken);

        var accessToken =
            _tokenService.GenerateAccessToken(user);

        var newRefreshToken =
            _tokenService.GenerateRefreshToken();

        var newRefreshTokenHash =
            _refreshTokenHasher.Hash(newRefreshToken.Token);

        var refreshTokenEntity = new RefreshToken(
            user.Id,
            newRefreshTokenHash,
            newRefreshToken.ExpiresAt);

        await _refreshTokenRepository.AddAsync(
            refreshTokenEntity,
            cancellationToken);

        return new LoginResponse
        {
            AccessToken = accessToken.Token,
            RefreshToken = newRefreshToken.Token,
            AccessTokenExpiresAt = accessToken.ExpiresAt,
            RefreshTokenExpiresAt = newRefreshToken.ExpiresAt
        };
    }
}