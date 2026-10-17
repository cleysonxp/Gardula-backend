
namespace Gardula.Application.Finance.RecurringTransactions.DTOs;

public record RecurringTransactionResponse(
    int Id,
    string Name,
    int Type,
    decimal Amount,
    int CategoryId,
    int PaymentMethod,
    int? AccountId,
    int? CardId,
    int DueDay,
    DateTimeOffset StartDate,
    DateTimeOffset? EndDate,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
