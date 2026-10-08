using Gardula.Application.Finance.Transactions.DTOs;
using Gardula.Application.Finance.Planning.DTOs;
using Gardula.Application.Finance.Cards.DTOs;

namespace Gardula.Application.Finance.Home.DTOs;

public record HomeOverviewResponse(
    int Year,
    int Month,
    decimal TotalBalance,
    TransactionSummaryResponse MonthlySummary,
    BudgetOverviewResponse Budget,
    List<CategorySpendingResponse> CategorySpending,
    List<FinancialGoalResponse> Goals,
    CardOverviewResponse Cards,
    List<TransactionListResponse> RecentTransactions
);