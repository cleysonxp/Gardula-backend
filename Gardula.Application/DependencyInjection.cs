using Gardula.Application.Authentication.Services;
using Gardula.Application.Finance.Accounts.Services;
using Gardula.Application.Finance.Cards.Services;
using Gardula.Application.Finance.Categories.Services;
using Gardula.Application.Finance.Transactions.Services;
using Gardula.Application.Finance.Transfers.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Gardula.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<RegisterService>();
        services.AddScoped<LoginService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<ILogoutService, LogoutService>();

        services.AddScoped<AccountService>();
        services.AddScoped<CardService>();
        services.AddScoped<CategoryService>();
        services.AddScoped<TransactionService>();
        services.AddScoped<TransferService>();

        return services;
    }
}