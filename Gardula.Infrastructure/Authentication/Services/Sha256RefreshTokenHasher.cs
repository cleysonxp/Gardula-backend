using System.Security.Cryptography;
using System.Text;
using Gardula.Application.Authentication.Services;

namespace Gardula.Infrastructure.Authentication.Services;

public class Sha256RefreshTokenHasher : IRefreshTokenHasher
{
    public string Hash(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        var bytes = Encoding.UTF8.GetBytes(token);

        var hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash);
    }
}