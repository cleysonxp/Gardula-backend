namespace Gardula.Domain.Entities.Finance;

public class Transaction
{
    public int Id { get; private set; }

    public int UserId { get; private set; }

    public int AccountId { get; private set; }

    public int? CategoryId { get; private set; }

    public int? TransferId { get; private set; }

    public decimal Amount { get; private set; }

    public TransactionType Type { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public DateTimeOffset Date { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public Transaction(
        int userId,
        int accountId,
        decimal amount,
        TransactionType type,
        string description,
        DateTimeOffset date,
        int? categoryId = null,
        int? transferId = null)
    {
        if (userId <= 0)
            throw new ArgumentException(
                "UserId must be greater than zero.",
                nameof(userId));

        if (accountId <= 0)
            throw new ArgumentException(
                "AccountId must be greater than zero.",
                nameof(accountId));

        if (amount <= 0)
            throw new ArgumentException(
                "Transaction amount must be greater than zero.",
                nameof(amount));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "Description is required.",
                nameof(description));

        if (categoryId.HasValue && categoryId.Value <= 0)
            throw new ArgumentException(
                "CategoryId must be greater than zero.",
                nameof(categoryId));

        if (transferId.HasValue && transferId.Value <= 0)
            throw new ArgumentException(
                "TransferId must be greater than zero.",
                nameof(transferId));

        if (type == TransactionType.Transfer && !transferId.HasValue)
            throw new ArgumentException(
                "TransferId is required for transfer transactions.",
                nameof(transferId));

        if (type != TransactionType.Transfer && transferId.HasValue)
            throw new ArgumentException(
                "TransferId can only be used for transfer transactions.",
                nameof(transferId));

        if (type == TransactionType.Transfer && categoryId.HasValue)
            throw new ArgumentException(
                "CategoryId cannot be used for transfer transactions.",
                nameof(categoryId));

        if (type != TransactionType.Transfer && !categoryId.HasValue)
            throw new ArgumentException(
                "CategoryId is required for income and expense transactions.",
                nameof(categoryId));

        UserId = userId;
        AccountId = accountId;
        Amount = amount;
        Type = type;
        Description = description;
        Date = date;
        CategoryId = categoryId;
        TransferId = transferId;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }
}