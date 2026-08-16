using Gardula.Application.Authentication.DTOs;

namespace Gardula.Application.Authentication.Services;

public interface ILogoutService
{
    Task ExecuteAsync(
        LogoutRequest request,
        CancellationToken cancellationToken = default);
}