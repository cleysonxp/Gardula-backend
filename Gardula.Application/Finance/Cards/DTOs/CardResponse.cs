namespace Gardula.Application.Finance.Cards.DTOs;

public record CardResponse(
    int Id,
    int AccountId,
    string Name,
    string LastFourDigits,
    decimal CreditLimit,
    int ClosingDay,
    int DueDay,
    int Brand,
    string Color,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);