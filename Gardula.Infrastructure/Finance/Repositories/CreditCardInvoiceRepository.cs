using Gardula.Application.Finance.Cards.Services;
using Gardula.Domain.Entities.Finance;
using Gardula.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gardula.Infrastructure.Finance.Repositories;

public class CreditCardInvoiceRepository : ICreditCardInvoiceRepository
{
    private readonly GardulaDbContext _context;

    public CreditCardInvoiceRepository(
        GardulaDbContext context)
    {
        _context = context;
    }

    public async Task<CreditCardInvoice?> GetByDateAsync(
        int userId,
        int cardId,
        DateTimeOffset date,
        CancellationToken cancellationToken = default)
    {
        return await _context.CreditCardInvoices
            .FirstOrDefaultAsync(
                invoice =>
                    invoice.UserId == userId &&
                    invoice.CardId == cardId &&
                    invoice.StartDate <= date &&
                    invoice.ClosingDate >= date,
                cancellationToken);
    }

    public async Task AddAsync(
        CreditCardInvoice invoice,
        CancellationToken cancellationToken = default)
    {
        await _context.CreditCardInvoices.AddAsync(
            invoice,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
