using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Categories.Services;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task<Category?> GetByIdAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Category category,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}