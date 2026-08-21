using Gardula.Application.Finance.Transfers.Services;
using Gardula.Domain.Entities.Finance;
using Gardula.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gardula.Infrastructure.Finance.Repositories;

public class TransferRepository : ITransferRepository
{
    private readonly GardulaDbContext _context;

    public TransferRepository(GardulaDbContext context)
    {
        _context = context;
    }

    public async Task<Transfer?> GetByIdAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Transfers
            .FirstOrDefaultAsync(
                transfer =>
                    transfer.Id == id &&
                    transfer.UserId == userId,
                cancellationToken);
    }

    public async Task AddAsync(
        Transfer transfer,
        CancellationToken cancellationToken = default)
    {
        await _context.Transfers.AddAsync(
            transfer,
            cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteAsync(
        Transfer transfer,
        CancellationToken cancellationToken = default)
    {
        _context.Transfers.Remove(transfer);

        return Task.CompletedTask;
    }
}