namespace Gardula.Application.Finance.Transactions.DTOs;

public record CardSummaryResponse(
    int Id,
    string Name,
    string LastFourDigits);