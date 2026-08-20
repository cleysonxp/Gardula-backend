using Gardula.Application.Finance.Transactions.DTOs;
using Gardula.Application.Finance.Transactions.Services;
using Gardula.Domain.Entities.Finance;
using Gardula.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gardula.Infrastructure.Finance.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly GardulaDbContext _context;

    public TransactionRepository(GardulaDbContext context)
    {
        _context = context;
    }

    public async Task<List<TransactionListItem>> GetAllByUserIdAsync(
        int userId,
        TransactionFilterRequest filter,
        CancellationToken cancellationToken = default)
    {
        var transactionQuery = _context.Transactions
            .Where(transaction =>
                transaction.UserId == userId);

        if (filter.StartDate.HasValue)
        {
            transactionQuery = transactionQuery.Where(transaction =>
                transaction.Date >= filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            transactionQuery = transactionQuery.Where(transaction =>
                transaction.Date <= filter.EndDate.Value);
        }

        if (filter.CategoryId.HasValue)
        {
            transactionQuery = transactionQuery.Where(transaction =>
                transaction.CategoryId == filter.CategoryId.Value);
        }

        if (filter.AccountId.HasValue)
        {
            transactionQuery = transactionQuery.Where(transaction =>
                transaction.AccountId == filter.AccountId.Value);
        }

        if (filter.CardId.HasValue)
        {
            transactionQuery = transactionQuery.Where(transaction =>
                transaction.CardId == filter.CardId.Value);
        }

        if (filter.Type.HasValue)
        {
            transactionQuery = transactionQuery.Where(transaction =>
                transaction.Type == filter.Type.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();

            transactionQuery = transactionQuery.Where(transaction =>
                transaction.Description.Contains(search));
        }

        var query =
            from transaction in transactionQuery

            join category in _context.Categories
                on transaction.CategoryId equals category.Id
                into categoryGroup
            from category in categoryGroup.DefaultIfEmpty()

            join account in _context.Accounts
                on transaction.AccountId equals account.Id
                into accountGroup
            from account in accountGroup.DefaultIfEmpty()

            join card in _context.Cards
                on transaction.CardId equals card.Id
                into cardGroup
            from card in cardGroup.DefaultIfEmpty()

            join transfer in _context.Transfers
                on transaction.TransferId equals transfer.Id
                into transferGroup
            from transfer in transferGroup.DefaultIfEmpty()

            join sourceAccount in _context.Accounts
                on transfer.SourceAccountId equals sourceAccount.Id
                into sourceAccountGroup
            from sourceAccount in sourceAccountGroup.DefaultIfEmpty()

            join destinationAccount in _context.Accounts
                on transfer.DestinationAccountId equals destinationAccount.Id
                into destinationAccountGroup
            from destinationAccount in destinationAccountGroup.DefaultIfEmpty()

            orderby transaction.Date descending

            select new TransactionListItem
            {
                Transaction = transaction,

                CategoryName = category != null
                    ? category.Name
                    : null,

                AccountName = account != null
                    ? account.Name
                    : null,

                CardName = card != null
                    ? card.Name
                    : null,

                CardLastFourDigits = card != null
                    ? card.LastFourDigits
                    : null,

                Transfer = transfer,

                SourceAccountName = sourceAccount != null
                    ? sourceAccount.Name
                    : null,

                DestinationAccountName = destinationAccount != null
                    ? destinationAccount.Name
                    : null
            };

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<TransactionSummaryResponse> GetSummaryAsync(
    int userId,
    TransactionFilterRequest filter,
    CancellationToken cancellationToken = default)
    {
        var query = _context.Transactions
            .Where(transaction =>
                transaction.UserId == userId);

        if (filter.StartDate.HasValue)
        {
            query = query.Where(transaction =>
                transaction.Date >= filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            query = query.Where(transaction =>
                transaction.Date <= filter.EndDate.Value);
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(transaction =>
                transaction.CategoryId == filter.CategoryId.Value);
        }

        if (filter.AccountId.HasValue)
        {
            query = query.Where(transaction =>
                transaction.AccountId == filter.AccountId.Value);
        }

        if (filter.CardId.HasValue)
        {
            query = query.Where(transaction =>
                transaction.CardId == filter.CardId.Value);
        }

        if (filter.Type.HasValue)
        {
            query = query.Where(transaction =>
                transaction.Type == filter.Type.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();

            query = query.Where(transaction =>
                transaction.Description.Contains(search));
        }

        var totalIncome = await query
            .Where(transaction =>
                transaction.Type == TransactionType.Income)
            .SumAsync(
                transaction => transaction.Amount,
                cancellationToken);

        var totalExpense = await query
            .Where(transaction =>
                transaction.Type == TransactionType.Expense)
            .SumAsync(
                transaction => transaction.Amount,
                cancellationToken);

        var totalTransactions = await query
            .Where(transaction =>
                transaction.Type != TransactionType.Transfer)
            .CountAsync(cancellationToken);

        return new TransactionSummaryResponse(
            totalIncome,
            totalExpense,
            totalIncome - totalExpense,
            totalTransactions);
    }

    public async Task<Transaction?> GetByIdAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .FirstOrDefaultAsync(
                transaction =>
                    transaction.Id == id &&
                    transaction.UserId == userId,
                cancellationToken);
    }

    public async Task AddAsync(
        Transaction transaction,
        CancellationToken cancellationToken = default)
    {
        await _context.Transactions.AddAsync(
            transaction,
            cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}