namespace Gardula.Application.Finance.Accounts.DTOs;

public record AccountOverviewFilterRequest(
    DateTimeOffset? StartDate = null,
    DateTimeOffset? EndDate = null,
    int? CategoryId = null,
    string? Search = null);