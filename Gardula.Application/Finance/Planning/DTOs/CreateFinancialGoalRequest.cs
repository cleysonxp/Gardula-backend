namespace Gardula.Application.Finance.Planning.DTOs;

public record CreateFinancialGoalRequest(
    string Name,
    string Description,
    decimal TargetAmount,
    DateTimeOffset TargetDate,
    string Icon);