namespace Gardula.Domain.Entities.Finance;

public class Account
{
    private static readonly string[] AllowedColors =
    [
        "violet",
        "blue",
        "green",
        "orange",
        "red",
        "pink",
        "amber",
        "slate"
    ];

    public int Id { get; private set; }

    public int UserId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public AccountType Type { get; private set; }

    public decimal InitialBalance { get; private set; }

    public string Color { get; private set; } = string.Empty;

    public bool IsActive { get; private set; } = true;

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public Account(
        int userId,
        string name,
        AccountType type,
        decimal initialBalance,
        string color)
    {
        if (userId <= 0)
            throw new ArgumentException(
                "UserId must be greater than zero.",
                nameof(userId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Name is required.",
                nameof(name));

        if (initialBalance < 0)
            throw new ArgumentException(
                "Initial balance cannot be negative.",
                nameof(initialBalance));

        if (!AllowedColors.Contains(color))
            throw new ArgumentException(
                "Invalid account color.",
                nameof(color));

        UserId = userId;
        Name = name;
        Type = type;
        InitialBalance = initialBalance;
        Color = color;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void Update(
        string name,
        AccountType type)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Name is required.",
                nameof(name));

        Name = name;
        Type = type;
        UpdatedAt = DateTimeOffset.UtcNow;
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