using Gardula.Application.Authentication.Services;
using Gardula.Infrastructure.Authentication.Configuration;
using Gardula.Infrastructure.Authentication.Repositories;
using Gardula.Infrastructure.Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Gardula.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.SectionName));

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration
                    .GetSection(JwtOptions.SectionName)
                    .Get<JwtOptions>()!;

                options.MapInboundClaims = false;

                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(
                                    jwtOptions.Secret)),

                        ValidateIssuer = false,
                        ValidateAudience = false,

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.Zero
                    };
            });

        services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<ITokenService, JwtTokenService>();

        services.AddScoped<IRefreshTokenHasher, Sha256RefreshTokenHasher>();

        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return services;
    }
}