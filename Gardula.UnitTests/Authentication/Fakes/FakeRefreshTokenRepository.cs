using Gardula.Application.Authentication.Services;
using Gardula.Domain.Entities.Authentication;

namespace Gardula.UnitTests.Authentication.Fakes;

public class FakeRefreshTokenRepository : IRefreshTokenRepository
{
    private readonly List<RefreshToken> _refreshTokens = [];

    public IReadOnlyCollection<RefreshToken> RefreshTokens =>
        _refreshTokens;

    public Task<RefreshToken?> GetByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        var token = _refreshTokens
            .FirstOrDefault(token => token.UserId == userId);

        return Task.FromResult(token);
    }

    public Task AddAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default)
    {
        _refreshTokens.Add(refreshToken);

        return Task.CompletedTask;
    }

    public Task UpdateAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}