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
            .Where(account => account.UserId == userId)
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
}