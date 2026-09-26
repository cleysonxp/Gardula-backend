using Gardula.Application.Common.Services;
using Gardula.Application.Finance.Planning.DTOs;

namespace Gardula.Application.Finance.Planning.Services;

public class FinancialGoalService
{
    private readonly IFinancialGoalRepository _financialGoalRepository;
    private readonly ICurrentUserService _currentUserService;

    public FinancialGoalService(
        IFinancialGoalRepository financialGoalRepository,
        ICurrentUserService currentUserService)
    {
        _financialGoalRepository = financialGoalRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<FinancialGoalResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var goals = await _financialGoalRepository.GetAllByUserIdAsync(
            userId,
            cancellationToken);

        return goals
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<FinancialGoalResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var goal = await _financialGoalRepository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (goal is null)
            return null;

        return MapToResponse(goal);
    }

    public async Task<FinancialGoalResponse> CreateAsync(
        CreateFinancialGoalRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var goal = new Domain.Entities.Finance.FinancialGoal(
            userId,
            request.Name,
            request.Description,
            request.TargetAmount,
            request.TargetDate,
            request.Icon);

        await _financialGoalRepository.AddAsync(
            goal,
            cancellationToken);

        return MapToResponse(goal);
    }

    public async Task<FinancialGoalResponse?> UpdateAsync(
        int id,
        UpdateFinancialGoalRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var goal = await _financialGoalRepository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (goal is null)
            return null;

        goal.Update(
            request.Name,
            request.Description,
            request.TargetAmount,
            request.TargetDate,
            request.Icon);

        await _financialGoalRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(goal);
    }

    public async Task<FinancialGoalResponse?> AddAmountAsync(
        int id,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var goal = await _financialGoalRepository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (goal is null)
            return null;

        goal.AddAmount(amount);

        await _financialGoalRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(goal);
    }

    private static FinancialGoalResponse MapToResponse(
        Domain.Entities.Finance.FinancialGoal goal)
    {
        var percentage = goal.TargetAmount > 0
            ? goal.CurrentAmount / goal.TargetAmount * 100
            : 0;

        return new FinancialGoalResponse(
            goal.Id,
            goal.Name,
            goal.Description,
            goal.CurrentAmount,
            goal.TargetAmount,
            percentage,
            goal.TargetDate,
            goal.Icon,
            goal.CurrentAmount >= goal.TargetAmount,
            goal.CreatedAt,
            goal.UpdatedAt);
    }
}