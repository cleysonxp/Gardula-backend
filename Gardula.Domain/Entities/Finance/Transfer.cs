namespace Gardula.Domain.Entities.Finance;

public class Transfer
{
    public int Id { get; private set; }

    public int UserId { get; private set; }

    public int SourceAccountId { get; private set; }

    public int DestinationAccountId { get; private set; }

    public decimal Amount { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public Transfer(
        int userId,
        int sourceAccountId,
        int destinationAccountId,
        decimal amount)
    {
        if (userId <= 0)
            throw new ArgumentException(
                "UserId must be greater than zero.",
                nameof(userId));

        if (sourceAccountId <= 0)
            throw new ArgumentException(
                "SourceAccountId must be greater than zero.",
                nameof(sourceAccountId));

        if (destinationAccountId <= 0)
            throw new ArgumentException(
                "DestinationAccountId must be greater than zero.",
                nameof(destinationAccountId));

        if (sourceAccountId == destinationAccountId)
            throw new ArgumentException(
                "Source and destination accounts must be different.",
                nameof(destinationAccountId));

        if (amount <= 0)
            throw new ArgumentException(
                "Transfer amount must be greater than zero.",
                nameof(amount));

        UserId = userId;
        SourceAccountId = sourceAccountId;
        DestinationAccountId = destinationAccountId;
        Amount = amount;

        CreatedAt = DateTimeOffset.UtcNow;
    }
}