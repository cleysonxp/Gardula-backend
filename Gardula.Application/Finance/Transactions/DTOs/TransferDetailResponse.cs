namespace Gardula.Application.Finance.Transactions.DTOs;

public record TransferDetailResponse(
    int Id,
    AccountSummaryResponse SourceAccount,
    AccountSummaryResponse DestinationAccount);