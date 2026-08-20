namespace Gardula.Application.Finance.Transactions.DTOs;

public record CreateTransactionRequest(
    int? AccountId,
    int? CardId,
    int? CategoryId,
    decimal Amount,
    int Type,
    int PaymentMethod,
    string Description,
    DateTimeOffset Date,
    Guid? InstallmentGroupId,
    int? InstallmentNumber,
    int? TotalInstallments);