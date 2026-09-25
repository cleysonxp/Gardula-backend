namespace Gardula.Application.Finance.Planning.DTOs;

public record CategorySpendingResponse(
    int CategoryId,
    string CategoryName,
    decimal Amount,
    decimal Percentage);