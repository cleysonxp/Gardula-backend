namespace Gardula.Application.Finance.Reports.DTOs;

public record ReportResponse(
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance,
    int TotalTransactions,
    IReadOnlyList<MonthlyEvolutionResponse> MonthlyEvolution,
    IReadOnlyList<ReportCategorySpendingResponse> CategorySpending);