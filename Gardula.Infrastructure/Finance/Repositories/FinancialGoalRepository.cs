using Gardula.Application.Finance.Planning.Services;
using Gardula.Domain.Entities.Finance;
using Gardula.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gardula.Infrastructure.Finance.Repositories;

public class FinancialGoalRepository : IFinancialGoalRepository
{
    private readonly GardulaDbContext _context;

    public FinancialGoalRepository(GardulaDbContext context)
    {
        _context = context;
    }

    public async Task<List<FinancialGoal>> GetAllByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.FinancialGoals
            .Where(goal => goal.UserId == userId)
            .OrderBy(goal => goal.TargetDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<FinancialGoal?> GetByIdAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.FinancialGoals
            .FirstOrDefaultAsync(
                goal =>
                    goal.Id == id &&
                    goal.UserId == userId,
                cancellationToken);
    }

    public async Task AddAsync(
        FinancialGoal goal,
        CancellationToken cancellationToken = default)
    {
        await _context.FinancialGoals.AddAsync(
            goal,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        FinancialGoal goal,
        CancellationToken cancellationToken = default)
    {
        _context.FinancialGoals.Remove(goal);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}