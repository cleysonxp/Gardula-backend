namespace Gardula.Application.Finance.Transactions.DTOs;

public record TransactionListResponse(
    int Id,
    string Description,
    decimal Amount,
    DateTimeOffset Date,
    int Type,
    string TypeName,
    int PaymentMethod,
    string PaymentMethodName,
    CategorySummaryResponse? Category,
    AccountSummaryResponse? Account,
    CardSummaryResponse? Card,
    InstallmentSummaryResponse? Installment,
    TransferSummaryResponse? Transfer);