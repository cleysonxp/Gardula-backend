namespace Gardula.Application.Finance.Cards.DTOs;

public record CreditCardInvoiceDetailResponse(
    int Id,
    int CardId,
    DateTimeOffset StartDate,
    DateTimeOffset ClosingDate,
    DateTimeOffset DueDate,
    decimal TotalAmount,
    int Status,
    DateTimeOffset? PaidAt,
    List<CreditCardInvoiceTransactionItem> Transactions);