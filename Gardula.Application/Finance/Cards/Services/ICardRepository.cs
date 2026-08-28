using Gardula.Application.Finance.Cards.DTOs;
using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Cards.Services;

public interface ICardRepository
{
    Task<List<Card>> GetAllByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task<Card?> GetByIdAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Card card,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task<CardOverviewResponse> GetOverviewByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default);
}