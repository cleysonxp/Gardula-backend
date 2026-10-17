
using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.RecurringTransactions.Services;

public interface IRecurringTransactionRepository
{
    Task<List<RecurringTransaction>> GetAllByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task<RecurringTransaction?> GetByIdAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        RecurringTransaction recurringTransaction,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        RecurringTransaction recurringTransaction,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
