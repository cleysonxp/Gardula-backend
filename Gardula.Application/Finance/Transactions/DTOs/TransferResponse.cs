namespace Gardula.Application.Finance.Transfers.DTOs;

public record TransferResponse(
    int Id,
    int SourceAccountId,
    int DestinationAccountId,
    decimal Amount,
    DateTimeOffset CreatedAt);