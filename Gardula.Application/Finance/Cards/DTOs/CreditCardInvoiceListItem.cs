namespace Gardula.Application.Finance.Cards.DTOs;

public record CreditCardInvoiceListItem(
    int Id,
    int CardId,
    DateTimeOffset StartDate,
    DateTimeOffset ClosingDate,
    DateTimeOffset DueDate,
    decimal TotalAmount,
    int Status);