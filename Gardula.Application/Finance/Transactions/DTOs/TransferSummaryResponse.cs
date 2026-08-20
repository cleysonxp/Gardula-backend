namespace Gardula.Application.Finance.Transactions.DTOs;

public record TransferSummaryResponse(
    int Id,
    int SourceAccountId,
    int DestinationAccountId);