using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Cards.Services;

public interface ICreditCardInvoiceRepository
{
    Task<CreditCardInvoice?> GetByDateAsync(
        int userId,
        int cardId,
        DateTimeOffset date,
        CancellationToken cancellationToken = default);

    Task<List<CreditCardInvoice>> GetAllByCardIdAsync(
        int userId,
        int cardId,
        CancellationToken cancellationToken = default);

    Task<CreditCardInvoice?> GetByIdAsync(
        int userId,
        int cardId,
        int invoiceId,
        CancellationToken cancellationToken = default);

    Task<List<Transaction>> GetTransactionsAsync(
        int userId,
        int cardId,
        int invoiceId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        CreditCardInvoice invoice,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}