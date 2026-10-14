namespace Gardula.Application.Finance.Cards.DTOs;

public record CardUsedLimitResponse(
    int CardId,
    decimal UsedLimit);