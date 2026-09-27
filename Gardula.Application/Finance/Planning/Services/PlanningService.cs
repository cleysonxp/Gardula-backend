using Gardula.Application.Common.Services;
using Gardula.Application.Finance.Planning.DTOs;
using Gardula.Application.Finance.Transactions.DTOs;
using Gardula.Application.Finance.Transactions.Services;

namespace Gardula.Application.Finance.Planning.Services;

public class PlanningService
{
    private readonly IMonthlyBudgetRepository _monthlyBudgetRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IFinancialGoalRepository _financialGoalRepository;
    private readonly ICurrentUserService _currentUserService;

    public PlanningService(
        IMonthlyBudgetRepository monthlyBudgetRepository,
        ITransactionRepository transactionRepository,
        IFinancialGoalRepository financialGoalRepository,
        ICurrentUserService currentUserService)
    {
        _monthlyBudgetRepository = monthlyBudgetRepository;
        _transactionRepository = transactionRepository;
        _financialGoalRepository = financialGoalRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PlanningOverviewResponse?> GetOverviewAsync(
        int year,
        int month,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var budget =
            await _monthlyBudgetRepository.GetLatestByUserIdAndPeriodAsync(
                userId,
                year,
                month,
                cancellationToken);

        if (budget is null)
            return null;

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

        var filter = new TransactionFilterRequest(
            StartDate: startDate,
            EndDate: endDate,
            CategoryId: null,
            AccountId: null,
            CardId: null,
            Type: null,
            Search: null,
            Page: 1,
            PageSize: 1,
            SortOrder: "desc");

        var summary = await _transactionRepository.GetSummaryAsync(
            userId,
            filter,
            cancellationToken);

        var categorySpending =
            await _transactionRepository.GetCategorySpendingAsync(
                userId,
                filter,
                cancellationToken);

        var goals = await _financialGoalRepository.GetAllByUserIdAsync(
            userId,
            cancellationToken);

        var spent = summary.TotalExpense;

        var available = budget.Amount - spent;

        var percentageUsed = budget.Amount > 0
            ? spent / budget.Amount * 100
            : 0;

        var categories = categorySpending
            .Select(item => new CategorySpendingResponse(
                item.CategoryId,
                item.CategoryName,
                item.Amount,
                spent > 0
                    ? item.Amount / spent * 100
                    : 0))
            .ToList();

        var now = DateTimeOffset.UtcNow;

        var remainingDays = 0;

        if (year > now.Year ||
            (year == now.Year && month > now.Month))
        {
            remainingDays = DateTime.DaysInMonth(year, month);
        }
        else if (year == now.Year && month == now.Month)
        {
            remainingDays = DateTime.DaysInMonth(year, month)
                - now.Day;
        }

        var dailyAverage = 0m;

        if (spent > 0)
        {
            if (year < now.Year ||
                (year == now.Year && month < now.Month))
            {
                var daysInMonth = DateTime.DaysInMonth(year, month);

                dailyAverage = spent / daysInMonth;
            }
            else if (year == now.Year && month == now.Month)
            {
                dailyAverage = spent / now.Day;
            }
        }

        var totalGoals = goals.Count;

        var activeGoals = goals.Count(goal =>
            goal.CurrentAmount < goal.TargetAmount);

        return new PlanningOverviewResponse(
            year,
            month,
            new BudgetOverviewResponse(
                budget.Amount,
                spent,
                available,
                percentageUsed),
            categories,
            new PlanningSummaryResponse(
                available,
                remainingDays,
                dailyAverage,
                activeGoals,
                totalGoals));
    }
}