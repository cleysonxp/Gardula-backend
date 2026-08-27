namespace Gardula.Domain.Entities.Finance;

public class CreditCardInvoicePayment
{
    public int Id { get; private set; }

    public int UserId { get; private set; }

    public int CreditCardInvoiceId { get; private set; }

    public int AccountId { get; private set; }

    public decimal Amount { get; private set; }

    public CreditCardInvoicePaymentMethod PaymentMethod { get; private set; }

    public DateTimeOffset PaidAt { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public CreditCardInvoicePayment(
        int userId,
        int creditCardInvoiceId,
        int accountId,
        decimal amount,
        CreditCardInvoicePaymentMethod paymentMethod)
    {
        if (userId <= 0)
            throw new ArgumentException(
                "UserId must be greater than zero.",
                nameof(userId));

        if (creditCardInvoiceId <= 0)
            throw new ArgumentException(
                "CreditCardInvoiceId must be greater than zero.",
                nameof(creditCardInvoiceId));

        if (accountId <= 0)
            throw new ArgumentException(
                "AccountId must be greater than zero.",
                nameof(accountId));

        if (amount <= 0)
            throw new ArgumentException(
                "Payment amount must be greater than zero.",
                nameof(amount));

        if (!Enum.IsDefined(paymentMethod))
            throw new ArgumentException(
                "Invalid payment method.",
                nameof(paymentMethod));

        UserId = userId;
        CreditCardInvoiceId = creditCardInvoiceId;
        AccountId = accountId;
        Amount = amount;
        PaymentMethod = paymentMethod;

        PaidAt = DateTimeOffset.UtcNow;
        CreatedAt = PaidAt;
        UpdatedAt = PaidAt;
    }
}