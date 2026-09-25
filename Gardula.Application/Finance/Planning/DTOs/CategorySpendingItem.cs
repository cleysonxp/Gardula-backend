namespace Gardula.Application.Finance.Planning.DTOs;

public record CategorySpendingItem(
    int CategoryId,
    string CategoryName,
    decimal Amount);