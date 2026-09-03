using Gardula.Application.Finance.Cards.DTOs;
using Gardula.Application.Finance.Cards.Services;
using Gardula.Domain.Entities.Finance;
using Gardula.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gardula.Infrastructure.Finance.Repositories;

public class CardRepository : ICardRepository
{
    private readonly GardulaDbContext _context;

    public CardRepository(GardulaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Card>> GetAllByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Cards
            .Where(card =>
                card.UserId == userId &&
                card.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<Card?> GetByIdAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Cards
            .FirstOrDefaultAsync(
                card =>
                    card.Id == id &&
                    card.UserId == userId,
                cancellationToken);
    }

    public async Task AddAsync(
        Card card,
        CancellationToken cancellationToken = default)
    {
        await _context.Cards.AddAsync(
            card,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<CardOverviewResponse> GetOverviewByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        var totalCards = await _context.Cards
            .Where(card =>
                card.UserId == userId &&
                card.IsActive)
            .CountAsync(cancellationToken);

        var totalCreditLimit = await _context.Cards
            .Where(card =>
                card.UserId == userId &&
                card.IsActive)
            .Select(card => (decimal?)card.CreditLimit)
            .SumAsync(cancellationToken) ?? 0m;

        var totalUsedLimit = await (
            from transaction in _context.Transactions

            join invoice in _context.CreditCardInvoices
                on transaction.CreditCardInvoiceId equals invoice.Id

            join card in _context.Cards
                on transaction.CardId equals card.Id

            where transaction.UserId == userId
                  && transaction.CardId.HasValue
                  && card.UserId == userId
                  && card.IsActive
                  && invoice.UserId == userId
                  && invoice.Status != CreditCardInvoiceStatus.Paid
                  && transaction.Type != TransactionType.CreditCardInvoicePayment

            select (decimal?)transaction.Amount
        ).SumAsync(cancellationToken) ?? 0m;

        var totalAvailableLimit =
            totalCreditLimit - totalUsedLimit;

        return new CardOverviewResponse(
            totalCards,
            totalCreditLimit,
            totalUsedLimit,
            totalAvailableLimit);
    }
}