using Gardula.Domain.Entities.Authentication;

namespace Gardula.Application.Authentication.Services;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task<RefreshToken?> GetByTokenHashAsync(
        string tokenHash,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default);
}