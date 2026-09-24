using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Planning.Services;

public interface IMonthlyBudgetRepository
{
    Task<MonthlyBudget?> GetByUserIdAndPeriodAsync(
        int userId,
        int year,
        int month,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        MonthlyBudget budget,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}