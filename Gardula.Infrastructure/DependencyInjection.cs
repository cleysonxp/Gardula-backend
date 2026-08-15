using Gardula.Application.Authentication.Services;
using Gardula.Infrastructure.Authentication.Repositories;
using Gardula.Infrastructure.Authentication.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Gardula.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}