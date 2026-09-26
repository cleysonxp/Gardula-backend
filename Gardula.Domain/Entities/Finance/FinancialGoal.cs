namespace Gardula.Domain.Entities.Finance;

public class FinancialGoal
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal CurrentAmount { get; private set; }
    public decimal TargetAmount { get; private set; }
    public DateTimeOffset TargetDate { get; private set; }
    public string Icon { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public FinancialGoal(
        int userId,
        string name,
        string description,
        decimal targetAmount,
        DateTimeOffset targetDate,
        string icon)
    {
        if (userId <= 0)
            throw new ArgumentException(
                "UserId must be greater than zero.",
                nameof(userId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Name is required.",
                nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "Description is required.",
                nameof(description));

        if (targetAmount <= 0)
            throw new ArgumentException(
                "Target amount must be greater than zero.",
                nameof(targetAmount));

        if (string.IsNullOrWhiteSpace(icon))
            throw new ArgumentException(
                "Icon is required.",
                nameof(icon));

        UserId = userId;
        Name = name.Trim();
        Description = description.Trim();
        CurrentAmount = 0;
        TargetAmount = targetAmount;
        TargetDate = targetDate;
        Icon = icon.Trim();

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void Update(
        string name,
        string description,
        decimal targetAmount,
        DateTimeOffset targetDate,
        string icon)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Name is required.",
                nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "Description is required.",
                nameof(description));

        if (targetAmount <= 0)
            throw new ArgumentException(
                "Target amount must be greater than zero.",
                nameof(targetAmount));

        if (string.IsNullOrWhiteSpace(icon))
            throw new ArgumentException(
                "Icon is required.",
                nameof(icon));

        Name = name.Trim();
        Description = description.Trim();
        TargetAmount = targetAmount;
        TargetDate = targetDate;
        Icon = icon.Trim();
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void AddAmount(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException(
                "Amount must be greater than zero.",
                nameof(amount));

        CurrentAmount += amount;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}