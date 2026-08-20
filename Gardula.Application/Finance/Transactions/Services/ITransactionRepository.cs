using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Transactions.Services;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetAllByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task<Transaction?> GetByIdAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Transaction transaction,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}