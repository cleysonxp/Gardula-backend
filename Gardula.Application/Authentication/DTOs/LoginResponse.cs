namespace Gardula.Application.Authentication.DTOs;

public class LoginResponse
{
    public string AccessToken { get; init; } = string.Empty;

    public string RefreshToken { get; init; } = string.Empty;

    public DateTimeOffset AccessTokenExpiresAt { get; init; }

    public DateTimeOffset RefreshTokenExpiresAt { get; init; }
}
