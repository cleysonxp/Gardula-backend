using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Transactions.DTOs;

public record TransactionFilterRequest(
    DateTimeOffset? StartDate = null,
    DateTimeOffset? EndDate = null,
    int? CategoryId = null,
    int? AccountId = null,
    int? CardId = null,
    TransactionType? Type = null,
    string? Search = null);