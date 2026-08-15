using Gardula.Application.Authentication.Services;

namespace Gardula.UnitTests.Authentication.Fakes;

public class FakePasswordHasher : IPasswordHasher
{
    public string? PasswordReceived { get; private set; }

    public string Hash(string password)
    {
        PasswordReceived = password;

        return $"HASHED:{password}";
    }

    public bool Verify(string password, string passwordHash)
    {
        return passwordHash == $"HASHED:{password}";
    }
}