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

    public async Task<List<Transaction>> GetAllByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Where(transaction =>
                transaction.UserId == userId)
            .OrderByDescending(transaction => transaction.Date)
            .ToListAsync(cancellationToken);
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