namespace Gardula.Application.Finance.Transfers.DTOs;

public record UpdateTransferRequest(
    int SourceAccountId,
    int DestinationAccountId,
    decimal Amount);