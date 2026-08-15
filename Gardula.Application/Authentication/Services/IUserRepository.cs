using Gardula.Domain.Entities.Authentication;

namespace Gardula.Application.Authentication.Services;

public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        User user,
        CancellationToken cancellationToken = default);
}