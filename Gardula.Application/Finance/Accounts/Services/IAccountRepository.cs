using Gardula.Application.Finance.Accounts.DTOs;
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

    Task<List<AccountBalanceItem>> GetBalancesByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task<List<AccountPeriodSummaryItem>> GetPeriodSummaryByUserIdAsync(
        int userId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        CancellationToken cancellationToken = default);

    Task<decimal> GetBalanceByAccountIdAsync(
        int accountId,
        int userId,
        CancellationToken cancellationToken = default);
}