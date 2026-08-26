using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Cards.Services;

public interface ICreditCardInvoiceRepository
{
    Task<CreditCardInvoice?> GetByDateAsync(
        int userId,
        int cardId,
        DateTimeOffset date,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        CreditCardInvoice invoice,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}