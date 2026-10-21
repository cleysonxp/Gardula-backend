namespace Gardula.Application.Finance.RecurringTransactions.DTOs;

public record CreateRecurringTransactionRequest(
    string Name,
    int Type,
    decimal Amount,
    int CategoryId,
    int PaymentMethod,
    int? AccountId,
    int? CardId,
    int DueDay,
    DateTimeOffset StartDate,
    DateTimeOffset? EndDate);
