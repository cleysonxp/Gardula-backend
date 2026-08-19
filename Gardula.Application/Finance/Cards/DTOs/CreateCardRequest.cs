namespace Gardula.Application.Finance.Cards.DTOs;

public record CreateCardRequest(
    int AccountId,
    string Name,
    string LastFourDigits,
    decimal CreditLimit,
    int ClosingDay,
    int DueDay,
    int Brand,
    string Color);