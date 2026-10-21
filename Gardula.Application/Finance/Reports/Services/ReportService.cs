using Gardula.Application.Common.Services;
using Gardula.Application.Finance.Reports.DTOs;
using Gardula.Application.Finance.Transactions.DTOs;
using Gardula.Application.Finance.Transactions.Services;

namespace Gardula.Application.Finance.Reports.Services;

public class ReportService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICurrentUserService _currentUserService;

    public ReportService(
        ITransactionRepository transactionRepository,
        ICurrentUserService currentUserService)
    {
        _transactionRepository = transactionRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ReportResponse> GetReportAsync(
        TransactionFilterRequest filter,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var summary = await _transactionRepository.GetSummaryAsync(
            userId,
            filter,
            cancellationToken);

        var monthlyEvolution = await _transactionRepository.GetMonthlyEvolutionAsync(
            userId,
            filter,
            cancellationToken);

        var categorySpending = await _transactionRepository.GetCategorySpendingAsync(
            userId,
            filter,
            cancellationToken);

        var totalCategorySpending = categorySpending.Sum(c => c.Amount);

        var categories = categorySpending
            .Select(c => new ReportCategorySpendingResponse(
                c.CategoryId,
                c.CategoryName,
                c.Amount,
                totalCategorySpending > 0
                    ? Math.Round(c.Amount / totalCategorySpending * 100m, 2)
                    : 0m))
            .ToList();

        return new ReportResponse(
            summary.TotalIncome,
            summary.TotalExpense,
            summary.Balance,
            summary.TotalTransactions,
            monthlyEvolution,
            categories);
    }
}
