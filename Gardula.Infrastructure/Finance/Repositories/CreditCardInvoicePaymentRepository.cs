using Gardula.Application.Finance.Cards.Services;
using Gardula.Domain.Entities.Finance;
using Gardula.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gardula.Infrastructure.Finance.Repositories;

public class CreditCardInvoicePaymentRepository
    : ICreditCardInvoicePaymentRepository
{
    private readonly GardulaDbContext _context;

    public CreditCardInvoicePaymentRepository(
        GardulaDbContext context)
    {
        _context = context;
    }

    public async Task<CreditCardInvoicePayment?> GetByInvoiceIdAsync(
        int userId,
        int invoiceId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CreditCardInvoicePayments
            .FirstOrDefaultAsync(
                payment =>
                    payment.UserId == userId &&
                    payment.CreditCardInvoiceId == invoiceId,
                cancellationToken);
    }

    public async Task AddAsync(
        CreditCardInvoicePayment payment,
        CancellationToken cancellationToken = default)
    {
        await _context.CreditCardInvoicePayments.AddAsync(
            payment,
            cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(
            cancellationToken);
    }
}