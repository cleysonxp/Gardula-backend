namespace Gardula.Application.Finance.Transactions.DTOs;

public record InstallmentSummaryResponse(
    Guid GroupId,
    int Number,
    int Total);