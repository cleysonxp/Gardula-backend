using Gardula.Application.Finance.Planning.Services;
using Gardula.Domain.Entities.Finance;
using Gardula.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gardula.Infrastructure.Finance.Repositories;

public class MonthlyBudgetRepository : IMonthlyBudgetRepository
{
    private readonly GardulaDbContext _context;

    public MonthlyBudgetRepository(GardulaDbContext context)
    {
        _context = context;
    }

    public async Task<MonthlyBudget?> GetByUserIdAndPeriodAsync(
        int userId,
        int year,
        int month,
        CancellationToken cancellationToken = default)
    {
        return await _context.MonthlyBudgets
            .FirstOrDefaultAsync(
                budget =>
                    budget.UserId == userId &&
                    budget.Year == year &&
                    budget.Month == month,
                cancellationToken);
    }

    public async Task AddAsync(
        MonthlyBudget budget,
        CancellationToken cancellationToken = default)
    {
        await _context.MonthlyBudgets.AddAsync(
            budget,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}