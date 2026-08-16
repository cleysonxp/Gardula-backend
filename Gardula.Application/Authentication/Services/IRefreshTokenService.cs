using Gardula.Application.Authentication.DTOs;

namespace Gardula.Application.Authentication.Services;

public interface IRefreshTokenService
{
    Task<LoginResponse> ExecuteAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken = default);
}