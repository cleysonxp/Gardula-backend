using Gardula.Application.Common.Services;
using Gardula.Application.Finance.Accounts.Services;
using Gardula.Application.Finance.Cards.DTOs;
using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Cards.Services;

public class CardService
{
    private readonly ICardRepository _cardRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentUserService _currentUserService;

    public CardService(
        ICardRepository cardRepository,
        IAccountRepository accountRepository,
        ICurrentUserService currentUserService)
    {
        _cardRepository = cardRepository;
        _accountRepository = accountRepository;
        _currentUserService = currentUserService;
    }

    public async Task<CardResponse> CreateAsync(
        CreateCardRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var account = await _accountRepository.GetByIdAsync(
            request.AccountId,
            userId,
            cancellationToken);

        if (account is null)
            throw new ArgumentException(
                "Account not found.",
                nameof(request.AccountId));

        var card = new Card(
            userId,
            request.AccountId,
            request.Name.Trim(),
            request.LastFourDigits.Trim(),
            request.CreditLimit,
            request.ClosingDay,
            request.DueDay,
            (CardBrand)request.Brand,
            request.Color.Trim());

        await _cardRepository.AddAsync(
            card,
            cancellationToken);

        return MapToResponse(card);
    }

    public async Task<List<CardResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var cards = await _cardRepository.GetAllByUserIdAsync(
            userId,
            cancellationToken);

        return cards
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<CardResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var card = await _cardRepository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (card is null)
            return null;

        return MapToResponse(card);
    }

    public async Task<CardResponse?> UpdateAsync(
        int id,
        UpdateCardRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var card = await _cardRepository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (card is null)
            return null;

        card.Update(
            request.Name.Trim(),
            request.CreditLimit,
            request.ClosingDay,
            request.DueDay,
            (CardBrand)request.Brand,
            request.Color.Trim());

        await _cardRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(card);
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var card = await _cardRepository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (card is null)
            return false;

        card.Deactivate();

        await _cardRepository.SaveChangesAsync(
            cancellationToken);

        return true;
    }

    private static CardResponse MapToResponse(Card card)
    {
        return new CardResponse(
            card.Id,
            card.AccountId,
            card.Name,
            card.LastFourDigits,
            card.CreditLimit,
            card.ClosingDay,
            card.DueDay,
            (int)card.Brand,
            card.Color,
            card.IsActive,
            card.CreatedAt,
            card.UpdatedAt);
    }
}