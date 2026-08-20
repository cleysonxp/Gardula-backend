namespace Gardula.Application.Finance.Transactions.DTOs;

public record TransactionResponse(
    int Id,
    int? AccountId,
    int? CardId,
    int? CategoryId,
    int? TransferId,
    decimal Amount,
    int Type,
    int PaymentMethod,
    string Description,
    DateTimeOffset Date,
    Guid? InstallmentGroupId,
    int? InstallmentNumber,
    int? TotalInstallments,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);