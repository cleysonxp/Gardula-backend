using Gardula.Application.Common.Services;
using Gardula.Application.Finance.Planning.DTOs;
using Gardula.Application.Finance.Planning.Services;
using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Planning;

public class MonthlyBudgetService
{
    private readonly IMonthlyBudgetRepository _monthlyBudgetRepository;
    private readonly ICurrentUserService _currentUserService;

    public MonthlyBudgetService(
        IMonthlyBudgetRepository monthlyBudgetRepository,
        ICurrentUserService currentUserService)
    {
        _monthlyBudgetRepository = monthlyBudgetRepository;
        _currentUserService = currentUserService;
    }

    public async Task<MonthlyBudgetResponse?> GetAsync(
        int year,
        int month,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var budget = await _monthlyBudgetRepository.GetByUserIdAndPeriodAsync(
            userId,
            year,
            month,
            cancellationToken);

        if (budget is null)
            return null;

        return new MonthlyBudgetResponse(
            budget.Id,
            budget.Year,
            budget.Month,
            budget.Amount,
            budget.CreatedAt,
            budget.UpdatedAt);
    }

    public async Task<MonthlyBudgetResponse> CreateAsync(
        int year,
        int month,
        CreateMonthlyBudgetRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var existingBudget =
            await _monthlyBudgetRepository.GetByUserIdAndPeriodAsync(
                userId,
                year,
                month,
                cancellationToken);

        if (existingBudget is not null)
            throw new InvalidOperationException(
                "A budget already exists for this period.");

        var budget = new MonthlyBudget(
            userId,
            year,
            month,
            request.Amount);

        await _monthlyBudgetRepository.AddAsync(
            budget,
            cancellationToken);

        return new MonthlyBudgetResponse(
            budget.Id,
            budget.Year,
            budget.Month,
            budget.Amount,
            budget.CreatedAt,
            budget.UpdatedAt);
    }

    public async Task<MonthlyBudgetResponse?> UpdateAsync(
        int year,
        int month,
        UpdateMonthlyBudgetRequest request,
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

        budget.UpdateAmount(request.Amount); ;

        await _monthlyBudgetRepository.SaveChangesAsync(
            cancellationToken);

        return new MonthlyBudgetResponse(
            budget.Id,
            budget.Year,
            budget.Month,
            budget.Amount,
            budget.CreatedAt,
            budget.UpdatedAt);
    }
}