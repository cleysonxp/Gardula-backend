namespace Gardula.Application.Finance.Planning.DTOs;

public record UpdateFinancialGoalRequest(
    string Name,
    string Description,
    decimal TargetAmount,
    DateTimeOffset TargetDate,
    string Icon);