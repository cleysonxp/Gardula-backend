using Gardula.Application.Finance.Accounts.DTOs;
using Gardula.Application.Finance.Accounts.Services;
using Gardula.Application.Finance.Cards.Services;
using Gardula.Application.Finance.Home.DTOs;
using Gardula.Application.Finance.Planning.DTOs;
using Gardula.Application.Finance.Planning.Services;
using Gardula.Application.Finance.Transactions.DTOs;
using Gardula.Application.Finance.Transactions.Services;

namespace Gardula.Application.Finance.Home.Services;

public class HomeService
{
    private readonly AccountService _accountService;
    private readonly TransactionService _transactionService;
    private readonly PlanningService _planningService;
    private readonly FinancialGoalService _financialGoalService;
    private readonly CardService _cardService;

    public HomeService(
        AccountService accountService,
        TransactionService transactionService,
        PlanningService planningService,
        FinancialGoalService financialGoalService,
        CardService cardService)
    {
        _accountService = accountService;
        _transactionService = transactionService;
        _planningService = planningService;
        _financialGoalService = financialGoalService;
        _cardService = cardService;
    }

    public async Task<HomeOverviewResponse> GetOverviewAsync(
        int year,
        int month,
        CancellationToken cancellationToken = default)
    {
        if (month < 1 || month > 12)
            throw new ArgumentOutOfRangeException(nameof(month));

        if (year < 1 || year > 9999)
            throw new ArgumentOutOfRangeException(nameof(year));

        var startDate = new DateTimeOffset(
            year,
            month,
            1,
            0,
            0,
            0,
            TimeSpan.Zero);

        var endDate = startDate
            .AddMonths(1)
            .AddTicks(-1);

        var accountOverview = await _accountService.GetOverviewAsync(
            new AccountOverviewFilterRequest(
                StartDate: startDate,
                EndDate: endDate),
            cancellationToken);

        var monthlySummary = await _transactionService.GetSummaryAsync(
            new TransactionFilterRequest(
                StartDate: startDate,
                EndDate: endDate),
            cancellationToken);

        var planningOverview = await _planningService.GetOverviewAsync(
            year,
            month,
            cancellationToken);

        var goals = await _financialGoalService.GetAllAsync(
            cancellationToken);

        var cardOverview = await _cardService.GetOverviewAsync(
            cancellationToken);

        var budget = planningOverview?.Budget
            ?? new BudgetOverviewResponse(
                0m,
                0m,
                0m,
                0m,
                false);

        var categorySpending = planningOverview?.CategorySpending
            ?? new List<CategorySpendingResponse>();

        return new HomeOverviewResponse(
            year,
            month,
            accountOverview.TotalBalance,
            monthlySummary,
            budget,
            categorySpending,
            goals,
            cardOverview,
            accountOverview.RecentTransactions);
    }
}