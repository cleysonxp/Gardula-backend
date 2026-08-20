namespace Gardula.Application.Finance.Categories.DTOs;

public record CreateCategoryRequest(
    string Name,
    int Type,
    int? ParentCategoryId);