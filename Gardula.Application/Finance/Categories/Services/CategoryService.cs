using Gardula.Application.Common.Services;
using Gardula.Application.Finance.Categories.DTOs;
using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Categories.Services;

public class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUserService;

    public CategoryService(
        ICategoryRepository categoryRepository,
        ICurrentUserService currentUserService)
    {
        _categoryRepository = categoryRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<CategoryResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var categories = await _categoryRepository.GetAllByUserIdAsync(
            userId,
            cancellationToken);

        return categories
            .Select(category => new CategoryResponse(
                category.Id,
                category.Name,
                (int)category.Type,
                category.ParentCategoryId,
                category.UserId,
                category.IsActive,
                category.CreatedAt,
                category.UpdatedAt))
            .ToList();
    }

    public async Task<CategoryResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var category = await _categoryRepository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (category is null)
            return null;

        return new CategoryResponse(
            category.Id,
            category.Name,
            (int)category.Type,
            category.ParentCategoryId,
            category.UserId,
            category.IsActive,
            category.CreatedAt,
            category.UpdatedAt);
    }

    public async Task<CategoryResponse> CreateAsync(
        CreateCategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var type = (CategoryType)request.Type;

        if (!Enum.IsDefined(type))
            throw new ArgumentException(
                "Invalid category type.",
                nameof(request.Type));

        if (request.ParentCategoryId.HasValue)
        {
            var parentCategory =
                await _categoryRepository.GetByIdAsync(
                    request.ParentCategoryId.Value,
                    userId,
                    cancellationToken);

            if (parentCategory is null)
                throw new ArgumentException(
                    "Parent category not found.",
                    nameof(request.ParentCategoryId));

            if (parentCategory.Type != type)
                throw new ArgumentException(
                    "Category type must match the parent category type.",
                    nameof(request.Type));
        }

        var category = new Category(
            userId,
            request.Name.Trim(),
            type,
            request.ParentCategoryId);

        await _categoryRepository.AddAsync(
            category,
            cancellationToken);

        return new CategoryResponse(
            category.Id,
            category.Name,
            (int)category.Type,
            category.ParentCategoryId,
            category.UserId,
            category.IsActive,
            category.CreatedAt,
            category.UpdatedAt);
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var category = await _categoryRepository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (category is null)
            return false;

        if (!category.UserId.HasValue)
            throw new InvalidOperationException(
                "System categories cannot be deleted.");

        category.Deactivate();

        await _categoryRepository.SaveChangesAsync(
            cancellationToken);

        return true;
    }
}