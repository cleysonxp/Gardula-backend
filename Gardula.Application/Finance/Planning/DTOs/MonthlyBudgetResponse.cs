namespace Gardula.Application.Finance.Planning.DTOs;

public record MonthlyBudgetResponse(
    int Id,
    int Year,
    int Month,
    decimal Amount,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);