namespace Gardula.Application.Authentication.Services;

public record GeneratedToken(
    string Token,
    DateTimeOffset ExpiresAt);