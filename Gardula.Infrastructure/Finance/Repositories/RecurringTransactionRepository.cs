
using Gardula.Application.Finance.RecurringTransactions.Services;
using Gardula.Domain.Entities.Finance;
using Gardula.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gardula.Infrastructure.Finance.Repositories;

public class RecurringTransactionRepository : IRecurringTransactionRepository
{
    private readonly GardulaDbContext _context;

    public RecurringTransactionRepository(GardulaDbContext context)
    {
        _context = context;
    }

    public async Task<List<RecurringTransaction>> GetAllByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.RecurringTransactions
            .Where(recurringTransaction =>
                recurringTransaction.UserId == userId)
            .OrderBy(recurringTransaction => recurringTransaction.DueDay)
            .ThenBy(recurringTransaction => recurringTransaction.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<RecurringTransaction?> GetByIdAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.RecurringTransactions
            .FirstOrDefaultAsync(
                recurringTransaction =>
                    recurringTransaction.Id == id &&
                    recurringTransaction.UserId == userId,
                cancellationToken);
    }

    public async Task AddAsync(
        RecurringTransaction recurringTransaction,
        CancellationToken cancellationToken = default)
    {
        await _context.RecurringTransactions.AddAsync(
            recurringTransaction,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        RecurringTransaction recurringTransaction,
        CancellationToken cancellationToken = default)
    {
        _context.RecurringTransactions.Remove(recurringTransaction);

        await _context.SaveChangesAsync(cancellationToken);
    }


    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
