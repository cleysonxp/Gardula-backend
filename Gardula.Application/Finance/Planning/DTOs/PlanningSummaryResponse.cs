namespace Gardula.Application.Finance.Planning.DTOs;

public record PlanningSummaryResponse(
    decimal AvailableToSpend,
    int RemainingDays,
    decimal DailyAverage,
    int ActiveGoals,
    int TotalGoals);