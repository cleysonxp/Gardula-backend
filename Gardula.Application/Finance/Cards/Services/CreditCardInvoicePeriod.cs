namespace Gardula.Application.Finance.Cards.Services;

public record CreditCardInvoicePeriod(
    DateTimeOffset StartDate,
    DateTimeOffset ClosingDate,
    DateTimeOffset DueDate);