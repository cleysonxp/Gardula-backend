using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Accounts.Services;

public interface IAccountRepository
{
    Task<List<Account>> GetAllByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task<Account?> GetByIdAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Account account,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}