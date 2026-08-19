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
}