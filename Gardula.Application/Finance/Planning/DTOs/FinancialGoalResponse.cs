namespace Gardula.Application.Finance.Planning.DTOs;

public record FinancialGoalResponse(
    int Id,
    string Name,
    string Description,
    decimal CurrentAmount,
    decimal TargetAmount,
    decimal Percentage,
    DateTimeOffset TargetDate,
    string Icon,
    bool IsCompleted,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);