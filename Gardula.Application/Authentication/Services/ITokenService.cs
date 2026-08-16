using Gardula.Domain.Entities.Authentication;

namespace Gardula.Application.Authentication.Services;

public interface ITokenService
{
    GeneratedToken GenerateAccessToken(User user);

    GeneratedToken GenerateRefreshToken();
}