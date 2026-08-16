using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Gardula.Application.Authentication.Services;
using Gardula.Domain.Entities.Authentication;
using Gardula.Infrastructure.Authentication.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Gardula.Infrastructure.Authentication.Services;

public class JwtTokenService : ITokenService
{
    private readonly JwtOptions _options;

    public JwtTokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public GeneratedToken GenerateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_options.Secret));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(
                JwtRegisteredClaimNames.Sub,
                user.Id.ToString()),

            new Claim(
                JwtRegisteredClaimNames.Email,
                user.Email),

            new Claim(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

        var expiresAt = DateTimeOffset.UtcNow
            .AddMinutes(_options.AccessTokenExpirationMinutes);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return new GeneratedToken(
            tokenString,
            expiresAt);
    }

    public GeneratedToken GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);

        var token = Convert.ToBase64String(randomBytes);

        var expiresAt = DateTimeOffset.UtcNow
            .AddDays(_options.RefreshTokenExpirationDays);

        return new GeneratedToken(
            token,
            expiresAt);
    }
}