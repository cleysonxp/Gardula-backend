namespace Gardula.Application.Finance.Accounts.DTOs;

public record AccountDetailFilterRequest(
    DateTimeOffset? StartDate = null,
    DateTimeOffset? EndDate = null,
    int? CategoryId = null,
    string? Search = null,
    int Page = 1,
    int PageSize = 10);