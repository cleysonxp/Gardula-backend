namespace Gardula.Application.Finance.Cards.DTOs;

public record UpdateCardRequest(
    string Name,
    decimal CreditLimit,
    int ClosingDay,
    int DueDay,
    int Brand,
    string Color);