namespace Gardula.Domain.Entities.Finance;

public class Transaction
{
    public int Id { get; private set; }

    public int UserId { get; private set; }

    public int? AccountId { get; private set; }

    public int? CardId { get; private set; }

    public int? CategoryId { get; private set; }

    public int? TransferId { get; private set; }

    public decimal Amount { get; private set; }

    public TransactionType Type { get; private set; }

    public PaymentMethod PaymentMethod { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public DateTimeOffset Date { get; private set; }

    public Guid? InstallmentGroupId { get; private set; }

    public int? InstallmentNumber { get; private set; }

    public int? TotalInstallments { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public Transaction(
        int userId,
        decimal amount,
        TransactionType type,
        PaymentMethod paymentMethod,
        string description,
        DateTimeOffset date,
        int? accountId = null,
        int? cardId = null,
        int? categoryId = null,
        int? transferId = null,
        Guid? installmentGroupId = null,
        int? installmentNumber = null,
        int? totalInstallments = null)
    {
        if (userId <= 0)
            throw new ArgumentException(
                "UserId must be greater than zero.",
                nameof(userId));

        if (amount <= 0)
            throw new ArgumentException(
                "Transaction amount must be greater than zero.",
                nameof(amount));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "Description is required.",
                nameof(description));

        if (accountId.HasValue && accountId.Value <= 0)
            throw new ArgumentException(
                "AccountId must be greater than zero.",
                nameof(accountId));

        if (cardId.HasValue && cardId.Value <= 0)
            throw new ArgumentException(
                "CardId must be greater than zero.",
                nameof(cardId));

        if (categoryId.HasValue && categoryId.Value <= 0)
            throw new ArgumentException(
                "CategoryId must be greater than zero.",
                nameof(categoryId));

        if (transferId.HasValue && transferId.Value <= 0)
            throw new ArgumentException(
                "TransferId must be greater than zero.",
                nameof(transferId));

        if (type == TransactionType.Transfer)
        {
            if (!transferId.HasValue)
                throw new ArgumentException(
                    "TransferId is required for transfer transactions.",
                    nameof(transferId));

            if (categoryId.HasValue)
                throw new ArgumentException(
                    "CategoryId cannot be used for transfer transactions.",
                    nameof(categoryId));
        }
        else
        {
            if (!categoryId.HasValue)
                throw new ArgumentException(
                    "CategoryId is required for income and expense transactions.",
                    nameof(categoryId));

            if (transferId.HasValue)
                throw new ArgumentException(
                    "TransferId can only be used for transfer transactions.",
                    nameof(transferId));
        }

        if (paymentMethod == PaymentMethod.CreditCard)
        {
            if (!cardId.HasValue)
                throw new ArgumentException(
                    "CardId is required for credit card transactions.",
                    nameof(cardId));

            if (accountId.HasValue)
                throw new ArgumentException(
                    "AccountId cannot be used directly for credit card transactions.",
                    nameof(accountId));
        }
        else
        {
            if (!accountId.HasValue && type != TransactionType.Transfer)
                throw new ArgumentException(
                    "AccountId is required for this payment method.",
                    nameof(accountId));

            if (cardId.HasValue)
                throw new ArgumentException(
                    "CardId can only be used for credit card transactions.",
                    nameof(cardId));
        }

        if (installmentNumber.HasValue || totalInstallments.HasValue)
        {
            if (!installmentGroupId.HasValue)
                throw new ArgumentException(
                    "InstallmentGroupId is required for installment transactions.",
                    nameof(installmentGroupId));

            if (!installmentNumber.HasValue || installmentNumber.Value <= 0)
                throw new ArgumentException(
                    "InstallmentNumber must be greater than zero.",
                    nameof(installmentNumber));

            if (!totalInstallments.HasValue || totalInstallments.Value <= 0)
                throw new ArgumentException(
                    "TotalInstallments must be greater than zero.",
                    nameof(totalInstallments));

            if (installmentNumber.Value > totalInstallments.Value)
                throw new ArgumentException(
                    "InstallmentNumber cannot be greater than TotalInstallments.",
                    nameof(installmentNumber));
        }

        UserId = userId;
        AccountId = accountId;
        CardId = cardId;
        CategoryId = categoryId;
        TransferId = transferId;
        Amount = amount;
        Type = type;
        PaymentMethod = paymentMethod;
        Description = description;
        Date = date;
        InstallmentGroupId = installmentGroupId;
        InstallmentNumber = installmentNumber;
        TotalInstallments = totalInstallments;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }
}