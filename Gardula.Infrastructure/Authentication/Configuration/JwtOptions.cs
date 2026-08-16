namespace Gardula.Infrastructure.Authentication.Configuration;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Secret { get; set; } = string.Empty;

    public int AccessTokenExpirationMinutes { get; set; }

    public int RefreshTokenExpirationDays { get; set; }
}