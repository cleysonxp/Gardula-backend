using Gardula.Application.Authentication.Services;

namespace Gardula.UnitTests.Authentication.Fakes;

public class FakeRefreshTokenHasher : IRefreshTokenHasher
{
    public string? TokenReceived { get; private set; }

    public string Hash(string token)
    {
        TokenReceived = token;

        return $"HASHED:{token}";
    }
}