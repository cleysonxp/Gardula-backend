namespace Gardula.Application.Authentication.DTOs;

public class LogoutRequest
{
    public string RefreshToken { get; init; } = string.Empty;
}