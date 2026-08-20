namespace Gardula.Application.Finance.Categories.DTOs;

public record CategoryResponse(
    int Id,
    string Name,
    int Type,
    int? ParentCategoryId,
    int? UserId,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);