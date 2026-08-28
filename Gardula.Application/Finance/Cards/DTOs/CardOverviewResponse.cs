namespace Gardula.Application.Finance.Cards.DTOs;

public record CardOverviewResponse(
    int TotalCards,
    decimal TotalCreditLimit,
    decimal TotalUsedLimit,
    decimal TotalAvailableLimit);