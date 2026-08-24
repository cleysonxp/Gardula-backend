using Gardula.Application.Finance.Accounts.DTOs;
using Gardula.Application.Finance.Accounts.Services;
using Gardula.Domain.Entities.Finance;
using Gardula.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gardula.Infrastructure.Finance.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly GardulaDbContext _context;

    public AccountRepository(GardulaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Account>> GetAllByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Accounts
            .Where(account =>
                account.UserId == userId &&
                account.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<Account?> GetByIdAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(
                account =>
                    account.Id == id &&
                    account.UserId == userId,
                cancellationToken);
    }

    public async Task AddAsync(
        Account account,
        CancellationToken cancellationToken = default)
    {
        await _context.Accounts.AddAsync(
            account,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<AccountBalanceItem>> GetBalancesByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        var transactionImpacts =
            from transaction in _context.Transactions

            join transfer in _context.Transfers
                on transaction.TransferId equals transfer.Id
                into transferGroup
            from transfer in transferGroup.DefaultIfEmpty()

            where transaction.UserId == userId
                  && transaction.AccountId.HasValue

            select new
            {
                AccountId = transaction.AccountId.Value,

                Amount =
                    transaction.Type == TransactionType.Income
                        ? transaction.Amount
                        : transaction.Type == TransactionType.Expense
                            ? -transaction.Amount
                            : transfer.SourceAccountId == transaction.AccountId.Value
                                ? -transaction.Amount
                                : transfer.DestinationAccountId == transaction.AccountId.Value
                                    ? transaction.Amount
                                    : 0m
            };

        var balances = await (
            from account in _context.Accounts

            where account.UserId == userId
                  && account.IsActive

            join impact in transactionImpacts
                on account.Id equals impact.AccountId
                into impactGroup

            select new AccountBalanceItem(
                account.Id,
                account.InitialBalance +
                impactGroup.Sum(x => x.Amount))
        )
        .ToListAsync(cancellationToken);

        return balances;
    }

    public async Task<List<AccountPeriodSummaryItem>> GetPeriodSummaryByUserIdAsync(
        int userId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        CancellationToken cancellationToken = default)
    {
        var result = await _context.Accounts
            .Where(account =>
                account.UserId == userId &&
                account.IsActive)
            .Select(account => new AccountPeriodSummaryItem(
                account.Id,

                _context.Transactions
                    .Where(transaction =>
                        transaction.UserId == userId &&
                        transaction.AccountId == account.Id &&
                        transaction.Date >= startDate &&
                        transaction.Date <= endDate &&
                        transaction.Type == TransactionType.Income)
                    .Select(transaction => (decimal?)transaction.Amount)
                    .Sum() ?? 0m,

                _context.Transactions
                    .Where(transaction =>
                        transaction.UserId == userId &&
                        transaction.AccountId == account.Id &&
                        transaction.Date >= startDate &&
                        transaction.Date <= endDate &&
                        transaction.Type == TransactionType.Expense)
                    .Select(transaction => (decimal?)transaction.Amount)
                    .Sum() ?? 0m
            ))
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<decimal> GetBalanceByAccountIdAsync(
        int accountId,
        int userId,
        CancellationToken cancellationToken = default)
    {
        var balance = await _context.Accounts
            .Where(account =>
                account.Id == accountId &&
                account.UserId == userId)
            .Select(account =>
                account.InitialBalance

                + (
                    _context.Transactions
                        .Where(transaction =>
                            transaction.UserId == userId &&
                            transaction.AccountId == accountId &&
                            transaction.Type == TransactionType.Income)
                        .Select(transaction => (decimal?)transaction.Amount)
                        .Sum() ?? 0m
                )

                - (
                    _context.Transactions
                        .Where(transaction =>
                            transaction.UserId == userId &&
                            transaction.AccountId == accountId &&
                            transaction.Type == TransactionType.Expense)
                        .Select(transaction => (decimal?)transaction.Amount)
                        .Sum() ?? 0m
                )

                - (
                    _context.Transactions
                        .Where(transaction =>
                            transaction.UserId == userId &&
                            transaction.AccountId == accountId &&
                            transaction.TransferId.HasValue &&
                            _context.Transfers.Any(transfer =>
                                transfer.Id == transaction.TransferId.Value &&
                                transfer.SourceAccountId == accountId))
                        .Select(transaction => (decimal?)transaction.Amount)
                        .Sum() ?? 0m
                )

                + (
                    _context.Transactions
                        .Where(transaction =>
                            transaction.UserId == userId &&
                            transaction.AccountId == accountId &&
                            transaction.TransferId.HasValue &&
                            _context.Transfers.Any(transfer =>
                                transfer.Id == transaction.TransferId.Value &&
                                transfer.DestinationAccountId == accountId))
                        .Select(transaction => (decimal?)transaction.Amount)
                        .Sum() ?? 0m
                ))
            .FirstOrDefaultAsync(cancellationToken);

        return balance;
    }
}