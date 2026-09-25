namespace Gardula.Application.Finance.Planning.DTOs;

public record PlanningOverviewResponse(
    int Year,
    int Month,
    BudgetOverviewResponse Budget,
    List<CategorySpendingResponse> CategorySpending,
    PlanningSummaryResponse Summary);