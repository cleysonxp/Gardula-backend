namespace Gardula.Application.Finance.Planning.DTOs;

public record BudgetOverviewResponse(
    decimal Amount,
    decimal Spent,
    decimal Available,
    decimal PercentageUsed);