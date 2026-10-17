namespace Gardula.Domain.Entities.Finance;

public class RecurringTransaction
{
    public int Id { get; private set; }

    public int UserId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public TransactionType Type { get; private set; }

    public decimal Amount { get; private set; }

    public int CategoryId { get; private set; }

    public PaymentMethod PaymentMethod { get; private set; }

    public int? AccountId { get; private set; }

    public int? CardId { get; private set; }

    public int DueDay { get; private set; }

    public DateTimeOffset StartDate { get; private set; }

    public DateTimeOffset? EndDate { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public RecurringTransaction(
        int userId,
        string name,
        TransactionType type,
        decimal amount,
        int categoryId,
        PaymentMethod paymentMethod,
        int? accountId,
        int? cardId,
        int dueDay,
        DateTimeOffset startDate,
        DateTimeOffset? endDate = null)
    {
        if (userId <= 0)
            throw new ArgumentException(
                "UserId must be greater than zero.",
                nameof(userId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Name is required.",
                nameof(name));

        if (type is not TransactionType.Income
            and not TransactionType.Expense)
            throw new ArgumentException(
                "Recurring transaction type must be Income or Expense.",
                nameof(type));

        if (amount <= 0)
            throw new ArgumentException(
                "Amount must be greater than zero.",
                nameof(amount));

        if (categoryId <= 0)
            throw new ArgumentException(
                "CategoryId must be greater than zero.",
                nameof(categoryId));

        if (!Enum.IsDefined(paymentMethod))
            throw new ArgumentException(
                "Invalid payment method.",
                nameof(paymentMethod));

        if (dueDay is < 1 or > 31)
            throw new ArgumentException(
                "DueDay must be between 1 and 31.",
                nameof(dueDay));

        if (endDate.HasValue && endDate.Value < startDate)
            throw new ArgumentException(
                "EndDate cannot be earlier than StartDate.",
                nameof(endDate));

        if (paymentMethod == PaymentMethod.CreditCard)
        {
            if (!cardId.HasValue || cardId.Value <= 0)
                throw new ArgumentException(
                    "CardId is required for credit card payments.",
                    nameof(cardId));

            if (accountId.HasValue)
                throw new ArgumentException(
                    "AccountId must not be provided for credit card payments.",
                    nameof(accountId));
        }
        else
        {
            if (!accountId.HasValue || accountId.Value <= 0)
                throw new ArgumentException(
                    "AccountId is required for this payment method.",
                    nameof(accountId));

            if (cardId.HasValue)
                throw new ArgumentException(
                    "CardId must not be provided for this payment method.",
                    nameof(cardId));
        }

        UserId = userId;
        Name = name.Trim();
        Type = type;
        Amount = amount;
        CategoryId = categoryId;
        PaymentMethod = paymentMethod;
        AccountId = accountId;
        CardId = cardId;
        DueDay = dueDay;
        StartDate = startDate;
        EndDate = endDate;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void Update(
        string name,
        TransactionType type,
        decimal amount,
        int categoryId,
        PaymentMethod paymentMethod,
        int? accountId,
        int? cardId,
        int dueDay,
        DateTimeOffset startDate,
        DateTimeOffset? endDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Name is required.",
                nameof(name));

        if (type is not TransactionType.Income
            and not TransactionType.Expense)
            throw new ArgumentException(
                "Recurring transaction type must be Income or Expense.",
                nameof(type));

        if (amount <= 0)
            throw new ArgumentException(
                "Amount must be greater than zero.",
                nameof(amount));

        if (categoryId <= 0)
            throw new ArgumentException(
                "CategoryId must be greater than zero.",
                nameof(categoryId));

        if (!Enum.IsDefined(paymentMethod))
            throw new ArgumentException(
                "Invalid payment method.",
                nameof(paymentMethod));

        if (dueDay is < 1 or > 31)
            throw new ArgumentException(
                "DueDay must be between 1 and 31.",
                nameof(dueDay));

        if (endDate.HasValue && endDate.Value < startDate)
            throw new ArgumentException(
                "EndDate cannot be earlier than StartDate.",
                nameof(endDate));

        if (paymentMethod == PaymentMethod.CreditCard)
        {
            if (!cardId.HasValue || cardId.Value <= 0)
                throw new ArgumentException(
                    "CardId is required for credit card payments.",
                    nameof(cardId));

            if (accountId.HasValue)
                throw new ArgumentException(
                    "AccountId must not be provided for credit card payments.",
                    nameof(accountId));
        }
        else
        {
            if (!accountId.HasValue || accountId.Value <= 0)
                throw new ArgumentException(
                    "AccountId is required for this payment method.",
                    nameof(accountId));

            if (cardId.HasValue)
                throw new ArgumentException(
                    "CardId must not be provided for this payment method.",
                    nameof(cardId));
        }

        Name = name.Trim();
        Type = type;
        Amount = amount;
        CategoryId = categoryId;
        PaymentMethod = paymentMethod;
        AccountId = accountId;
        CardId = cardId;
        DueDay = dueDay;
        StartDate = startDate;
        EndDate = endDate;
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
