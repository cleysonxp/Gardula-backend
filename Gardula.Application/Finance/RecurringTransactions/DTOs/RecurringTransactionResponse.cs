namespace Gardula.Application.Finance.RecurringTransactions.DTOs;

public record RecurringTransactionResponse(
    int Id,
    string Name,
    int Type,
    decimal Amount,
    int CategoryId,
    string CategoryName,
    int PaymentMethod,
    int? AccountId,
    string? AccountName,
    int? CardId,
    string? CardName,
    int DueDay,
    DateTimeOffset StartDate,
    DateTimeOffset? EndDate,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
