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

    public async Task<List<CreditCardInvoice>> GetAllByCardIdAsync(
        int userId,
        int cardId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CreditCardInvoices
            .Where(invoice =>
                invoice.UserId == userId &&
                invoice.CardId == cardId)
            .OrderByDescending(invoice => invoice.ClosingDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<CreditCardInvoice?> GetByIdAsync(
        int userId,
        int cardId,
        int invoiceId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CreditCardInvoices
            .FirstOrDefaultAsync(
                invoice =>
                    invoice.Id == invoiceId &&
                    invoice.UserId == userId &&
                    invoice.CardId == cardId,
                cancellationToken);
    }

    public async Task<List<Transaction>> GetTransactionsAsync(
        int userId,
        int cardId,
        int invoiceId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Where(transaction =>
                transaction.UserId == userId &&
                transaction.CardId == cardId &&
                transaction.CreditCardInvoiceId == invoiceId)
            .OrderByDescending(transaction => transaction.Date)
            .ToListAsync(cancellationToken);
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

    public async Task DeleteAsync(
        CreditCardInvoice invoice,
        CancellationToken cancellationToken = default)
    {
        _context.CreditCardInvoices.Remove(invoice);

        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<CreditCardInvoice>> GetAllByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CreditCardInvoices
            .Where(invoice =>
                invoice.UserId == userId)
            .OrderByDescending(invoice => invoice.ClosingDate)
            .ToListAsync(cancellationToken);
    }
}