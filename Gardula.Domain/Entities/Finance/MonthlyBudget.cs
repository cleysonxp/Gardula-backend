namespace Gardula.Domain.Entities.Finance;

public class MonthlyBudget
{
    public int Id { get; private set; }

    public int UserId { get; private set; }

    public int Year { get; private set; }

    public int Month { get; private set; }

    public decimal Amount { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public MonthlyBudget(
        int userId,
        int year,
        int month,
        decimal amount)
    {
        if (userId <= 0)
            throw new ArgumentException(
                "UserId must be greater than zero.",
                nameof(userId));

        if (year <= 0)
            throw new ArgumentException(
                "Year must be greater than zero.",
                nameof(year));

        if (month is < 1 or > 12)
            throw new ArgumentException(
                "Month must be between 1 and 12.",
                nameof(month));

        if (amount < 0)
            throw new ArgumentException(
                "Amount cannot be negative.",
                nameof(amount));

        UserId = userId;
        Year = year;
        Month = month;
        Amount = amount;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void UpdateAmount(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException(
                "Amount cannot be negative.",
                nameof(amount));

        Amount = amount;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}