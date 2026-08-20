using Gardula.Application.Finance.Categories.Services;
using Gardula.Domain.Entities.Finance;
using Gardula.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gardula.Infrastructure.Finance.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly GardulaDbContext _context;

    public CategoryRepository(GardulaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Where(category =>
                category.IsActive &&
                (
                    category.UserId == null ||
                    category.UserId == userId
                ))
            .OrderBy(category => category.Name)
            .ThenBy(category => category.ParentCategoryId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(
        int id,
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(
                category =>
                    category.Id == id &&
                    category.IsActive &&
                    (
                        category.UserId == null ||
                        category.UserId == userId
                    ),
                cancellationToken);
    }

    public async Task AddAsync(
        Category category,
        CancellationToken cancellationToken = default)
    {
        await _context.Categories.AddAsync(
            category,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}