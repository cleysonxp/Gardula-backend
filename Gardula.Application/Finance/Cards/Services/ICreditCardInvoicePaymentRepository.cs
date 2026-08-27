using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Cards.Services;

public interface ICreditCardInvoicePaymentRepository
{
    Task<CreditCardInvoicePayment?> GetByInvoiceIdAsync(
        int userId,
        int invoiceId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        CreditCardInvoicePayment payment,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}