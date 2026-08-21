using Gardula.Application.Finance.Transactions.DTOs;
using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Transactions.Services;

public interface ITransactionRepository
{
    Task<List<TransactionListItem>> GetAllByUserIdAsync(
        int userId,
        TransactionFilterRequest filter,
        CancellationToken cancellationToken = default);

    Task<TransactionSummaryResponse> GetSummaryAsync(
        int userId,
        TransactionFilterRequest filter,
        CancellationToken cancellationToken = default);

    Task<Transaction?> GetByIdAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default);

    Task<Transaction?> GetInstallmentByIdAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Transaction transaction,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Transaction transaction,
        CancellationToken cancellationToken = default);

    Task DeleteInstallmentGroupAsync(
        Guid installmentGroupId,
        int userId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Transaction transaction,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task<TransactionDetailResponse?> GetDetailByIdAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default);

    Task<List<Transaction>> GetInstallmentsByGroupIdAsync(
        Guid installmentGroupId,
        int userId,
        CancellationToken cancellationToken = default);

    Task<List<Transaction>> GetByTransferIdAsync(
        int transferId,
        int userId,
        CancellationToken cancellationToken = default);

    Task DeleteByTransferIdAsync(
        int transferId,
        int userId,
        CancellationToken cancellationToken = default);

}