namespace Gardula.Application.Finance.Reports.DTOs;

public record MonthlyEvolutionResponse(
    int Year,
    int Month,
    decimal TotalIncome,
    decimal TotalExpense);