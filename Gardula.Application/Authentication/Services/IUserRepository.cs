using Gardula.Domain.Entities.Authentication;

namespace Gardula.Application.Authentication.Services;

public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        User user,
        CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);
}