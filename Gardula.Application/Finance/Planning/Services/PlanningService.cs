using Gardula.Application.Common.Services;
using Gardula.Application.Finance.Planning.DTOs;
using Gardula.Application.Finance.Planning.Services;
using Gardula.Application.Finance.Transactions.DTOs;
using Gardula.Application.Finance.Transactions.Services;

namespace Gardula.Application.Finance.Planning;

public class PlanningService
{
    private readonly IMonthlyBudgetRepository _monthlyBudgetRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICurrentUserService _currentUserService;

    public PlanningService(
        IMonthlyBudgetRepository monthlyBudgetRepository,
        ITransactionRepository transactionRepository,
        ICurrentUserService currentUserService)
    {
        _monthlyBudgetRepository = monthlyBudgetRepository;
        _transactionRepository = transactionRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PlanningOverviewResponse?> GetOverviewAsync(
        int year,
        int month,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var budget =
            await _monthlyBudgetRepository.GetByUserIdAndPeriodAsync(
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
                0,
                0,
                0,
                0));
    }
}