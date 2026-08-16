using Gardula.Application.Authentication.Services;
using Gardula.Domain.Entities.Authentication;

namespace Gardula.UnitTests.Authentication.Fakes;

public class FakeTokenService : ITokenService
{
    public GeneratedToken GenerateAccessToken(User user)
    {
        return new GeneratedToken(
            "fake-access-token",
            DateTimeOffset.UtcNow.AddMinutes(30));
    }

    public GeneratedToken GenerateRefreshToken()
    {
        return new GeneratedToken(
            "fake-refresh-token",
            DateTimeOffset.UtcNow.AddDays(7));
    }
}