namespace Gardula.Application.Finance.Transactions.DTOs;

public record InstallmentDetailResponse(
    Guid GroupId,
    int Number,
    int Total,
    decimal TotalAmount,
    DateTimeOffset FirstDate);