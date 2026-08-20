namespace Gardula.Application.Finance.Transfers.DTOs;

public record CreateTransferRequest(
    int SourceAccountId,
    int DestinationAccountId,
    decimal Amount);