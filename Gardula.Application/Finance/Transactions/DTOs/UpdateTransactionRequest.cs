namespace Gardula.Application.Finance.Transactions.DTOs;

public record UpdateTransactionRequest(
    decimal Amount,
    int Type,
    string Description,
    DateTimeOffset Date,
    int CategoryId,
    int? AccountId,
    int? CardId,
    int PaymentMethod);