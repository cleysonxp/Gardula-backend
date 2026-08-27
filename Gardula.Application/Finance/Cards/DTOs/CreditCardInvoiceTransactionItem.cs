namespace Gardula.Application.Finance.Cards.DTOs;

public record CreditCardInvoiceTransactionItem(
    int Id,
    string Description,
    decimal Amount,
    DateTimeOffset Date,
    int? InstallmentNumber,
    int? TotalInstallments);