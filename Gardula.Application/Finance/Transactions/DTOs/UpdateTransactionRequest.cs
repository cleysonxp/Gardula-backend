namespace Gardula.Application.Finance.Transactions.DTOs;

public record UpdateTransactionRequest(
    decimal Amount,
    string Description,
    DateTimeOffset Date,
    int CategoryId,
    int? AccountId,
    int? CardId,
    int PaymentMethod);