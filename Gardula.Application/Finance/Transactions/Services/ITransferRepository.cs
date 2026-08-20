using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Transfers.Services;

public interface ITransferRepository
{
    Task<Transfer?> GetByIdAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Transfer transfer,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}