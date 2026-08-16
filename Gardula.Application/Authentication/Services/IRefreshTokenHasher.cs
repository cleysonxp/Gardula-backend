namespace Gardula.Application.Authentication.Services;

public interface IRefreshTokenHasher
{
    string Hash(string token);
}