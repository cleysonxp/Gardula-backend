namespace Gardula.Domain.Entities.Finance;

public class Category
{
    public int Id { get; private set; }

    public int? UserId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public CategoryType Type { get; private set; }

    public int? ParentCategoryId { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public Category(
    int? userId,
    string name,
    CategoryType type,
    int? parentCategoryId = null)
    {
        if (userId.HasValue && userId.Value <= 0)
            throw new ArgumentException(
                "UserId must be greater than zero.",
                nameof(userId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Name is required.",
                nameof(name));

        if (parentCategoryId.HasValue && parentCategoryId.Value <= 0)
            throw new ArgumentException(
                "ParentCategoryId must be greater than zero.",
                nameof(parentCategoryId));

        UserId = userId;
        Name = name;
        Type = type;
        ParentCategoryId = parentCategoryId;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}