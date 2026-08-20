using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Transactions.DTOs;

public class TransactionListItem
{
    public Transaction Transaction { get; init; } = null!;

    public string? CategoryName { get; init; }

    public string? AccountName { get; init; }

    public string? CardName { get; init; }

    public string? CardLastFourDigits { get; init; }

    public Transfer? Transfer { get; init; }

    public string? SourceAccountName { get; init; }

    public string? DestinationAccountName { get; init; }
}