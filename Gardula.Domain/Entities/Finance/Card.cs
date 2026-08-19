namespace Gardula.Domain.Entities.Finance;

public class Card
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

    public int AccountId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string LastFourDigits { get; private set; } = string.Empty;

    public decimal CreditLimit { get; private set; }

    public int ClosingDay { get; private set; }

    public int DueDay { get; private set; }

    public CardBrand Brand { get; private set; }

    public string Color { get; private set; } = string.Empty;

    public bool IsActive { get; private set; } = true;

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public Card(
        int userId,
        int accountId,
        string name,
        string lastFourDigits,
        decimal creditLimit,
        int closingDay,
        int dueDay,
        CardBrand brand,
        string color)
    {
        if (userId <= 0)
            throw new ArgumentException(
                "UserId must be greater than zero.",
                nameof(userId));

        if (accountId <= 0)
            throw new ArgumentException(
                "AccountId must be greater than zero.",
                nameof(accountId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Name is required.",
                nameof(name));

        if (string.IsNullOrWhiteSpace(lastFourDigits))
            throw new ArgumentException(
                "Last four digits are required.",
                nameof(lastFourDigits));

        if (lastFourDigits.Length != 4 ||
            !lastFourDigits.All(char.IsDigit))
            throw new ArgumentException(
                "Last four digits must contain exactly four numbers.",
                nameof(lastFourDigits));

        if (creditLimit <= 0)
            throw new ArgumentException(
                "Credit limit must be greater than zero.",
                nameof(creditLimit));

        if (closingDay is < 1 or > 31)
            throw new ArgumentException(
                "Closing day must be between 1 and 31.",
                nameof(closingDay));

        if (dueDay is < 1 or > 31)
            throw new ArgumentException(
                "Due day must be between 1 and 31.",
                nameof(dueDay));

        if (!AllowedColors.Contains(color))
            throw new ArgumentException(
                "Invalid card color.",
                nameof(color));

        UserId = userId;
        AccountId = accountId;
        Name = name;
        LastFourDigits = lastFourDigits;
        CreditLimit = creditLimit;
        ClosingDay = closingDay;
        DueDay = dueDay;
        Brand = brand;
        Color = color;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void Update(
        string name,
        decimal creditLimit,
        int closingDay,
        int dueDay,
        CardBrand brand,
        string color)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Name is required.",
                nameof(name));

        if (creditLimit <= 0)
            throw new ArgumentException(
                "Credit limit must be greater than zero.",
                nameof(creditLimit));

        if (closingDay is < 1 or > 31)
            throw new ArgumentException(
                "Closing day must be between 1 and 31.",
                nameof(closingDay));

        if (dueDay is < 1 or > 31)
            throw new ArgumentException(
                "Due day must be between 1 and 31.",
                nameof(dueDay));

        if (!AllowedColors.Contains(color))
            throw new ArgumentException(
                "Invalid card color.",
                nameof(color));

        Name = name;
        CreditLimit = creditLimit;
        ClosingDay = closingDay;
        DueDay = dueDay;
        Brand = brand;
        Color = color;

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