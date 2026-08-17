using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Gardula.Application.Common.Services;

namespace Gardula.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (!int.TryParse(userId, out var parsedUserId))
                throw new UnauthorizedAccessException(
                    "Authenticated user ID is missing or invalid.");

            return parsedUserId;
        }
    }
}