namespace Gardula.Application.Finance.Transactions.DTOs;

public record TransactionDetailResponse(
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
    InstallmentDetailResponse? Installment,
    TransferDetailResponse? Transfer,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);