using Gardula.Application.Authentication.DTOs;
using Gardula.Application.Authentication.Exceptions;
using Gardula.Domain.Entities.Authentication;

namespace Gardula.Application.Authentication.Services;

public class LoginService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenHasher _refreshTokenHasher;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LoginService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IRefreshTokenHasher refreshTokenHasher,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _refreshTokenHasher = refreshTokenHasher;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<LoginResponse> ExecuteAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _userRepository.GetByEmailAsync(
            email,
            cancellationToken);

        if (user is null)
            throw new InvalidCredentialsException();

        if (!user.IsActive)
            throw new InvalidCredentialsException();

        var passwordIsValid = _passwordHasher.Verify(
            request.Password,
            user.PasswordHash);

        if (!passwordIsValid)
            throw new InvalidCredentialsException();

        var accessToken = _tokenService.GenerateAccessToken(user);

        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenHash =
            _refreshTokenHasher.Hash(refreshToken.Token);

        var existingRefreshToken =
            await _refreshTokenRepository.GetByUserIdAsync(
                user.Id,
                cancellationToken);

        if (existingRefreshToken is not null)
        {
            existingRefreshToken.Revoke();

            await _refreshTokenRepository.UpdateAsync(
                existingRefreshToken,
                cancellationToken);
        }

        var newRefreshToken = new RefreshToken(
            user.Id,
            refreshTokenHash,
            refreshToken.ExpiresAt);

        await _refreshTokenRepository.AddAsync(
            newRefreshToken,
            cancellationToken);

        return new LoginResponse
        {
            AccessToken = accessToken.Token,
            RefreshToken = refreshToken.Token,
            AccessTokenExpiresAt = accessToken.ExpiresAt,
            RefreshTokenExpiresAt = refreshToken.ExpiresAt
        };
    }
}