namespace Gardula.Domain.Entities.Finance;

public class CreditCardInvoice
{
    public int Id { get; private set; }

    public int UserId { get; private set; }

    public int CardId { get; private set; }

    public DateTimeOffset StartDate { get; private set; }

    public DateTimeOffset ClosingDate { get; private set; }

    public DateTimeOffset DueDate { get; private set; }

    public CreditCardInvoiceStatus Status { get; private set; }

    public DateTimeOffset? PaidAt { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public CreditCardInvoice(
        int userId,
        int cardId,
        DateTimeOffset startDate,
        DateTimeOffset closingDate,
        DateTimeOffset dueDate)
    {
        if (userId <= 0)
            throw new ArgumentException(
                "UserId must be greater than zero.",
                nameof(userId));

        if (cardId <= 0)
            throw new ArgumentException(
                "CardId must be greater than zero.",
                nameof(cardId));

        if (startDate > closingDate)
            throw new ArgumentException(
                "Start date cannot be greater than closing date.",
                nameof(startDate));

        if (dueDate < closingDate)
            throw new ArgumentException(
                "Due date cannot be before closing date.",
                nameof(dueDate));

        UserId = userId;
        CardId = cardId;
        StartDate = startDate;
        ClosingDate = closingDate;
        DueDate = dueDate;
        Status = CreditCardInvoiceStatus.Open;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void MarkAsPaid()
    {
        if (Status == CreditCardInvoiceStatus.Paid)
            return;

        Status = CreditCardInvoiceStatus.Paid;
        PaidAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}