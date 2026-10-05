using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Planning.Services;

public interface IFinancialGoalRepository
{
    Task<List<FinancialGoal>> GetAllByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task<FinancialGoal?> GetByIdAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        FinancialGoal goal,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        FinancialGoal goal,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}