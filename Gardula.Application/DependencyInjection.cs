using Gardula.Application.Authentication.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Gardula.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<RegisterService>();
        services.AddScoped<LoginService>();

        return services;
    }
}