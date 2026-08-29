using Gardula.Application.Common.Services;
using Gardula.Application.Finance.Accounts.Services;
using Gardula.Application.Finance.Transactions.Services;
using Gardula.Application.Finance.Transfers.DTOs;
using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Transfers.Services;

public class TransferService
{
    private readonly ITransferRepository _transferRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentUserService _currentUserService;

    public TransferService(
        ITransferRepository transferRepository,
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository,
        ICurrentUserService currentUserService)
    {
        _transferRepository = transferRepository;
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _currentUserService = currentUserService;
    }

    public async Task<TransferResponse> CreateAsync(
        CreateTransferRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        if (request.SourceAccountId == request.DestinationAccountId)
        {
            throw new ArgumentException(
                "Source and destination accounts must be different.",
                nameof(request.DestinationAccountId));
        }

        if (request.Amount <= 0)
        {
            throw new ArgumentException(
                "Transfer amount must be greater than zero.",
                nameof(request.Amount));
        }

        var sourceAccount = await _accountRepository.GetByIdAsync(
            request.SourceAccountId,
            userId,
            cancellationToken);

        if (sourceAccount is null)
        {
            throw new ArgumentException(
                "Source account not found.",
                nameof(request.SourceAccountId));
        }

        if (!sourceAccount.IsActive)
        {
            throw new ArgumentException(
                "Source account is inactive.",
                nameof(request.SourceAccountId));
        }

        if (sourceAccount.CurrentBalance < request.Amount)
        {
            throw new InvalidOperationException(
                "Insufficient funds.");
        }

        var destinationAccount = await _accountRepository.GetByIdAsync(
            request.DestinationAccountId,
            userId,
            cancellationToken);

        if (destinationAccount is null)
        {
            throw new ArgumentException(
                "Destination account not found.",
                nameof(request.DestinationAccountId));
        }

        if (!destinationAccount.IsActive)
        {
            throw new ArgumentException(
                "Destination account is inactive.",
                nameof(request.DestinationAccountId));
        }

        var transfer = new Transfer(
            userId,
            request.SourceAccountId,
            request.DestinationAccountId,
            request.Amount);

        sourceAccount.Debit(request.Amount);
        destinationAccount.Credit(request.Amount);

        await _transferRepository.AddAsync(
            transfer,
            cancellationToken);

        await _transferRepository.SaveChangesAsync(
            cancellationToken);

        var sourceTransaction = new Transaction(
            userId,
            request.Amount,
            TransactionType.Transfer,
            PaymentMethod.Other,
            "Transferência enviada",
            DateTimeOffset.UtcNow,
            accountId: request.SourceAccountId,
            transferId: transfer.Id);

        var destinationTransaction = new Transaction(
            userId,
            request.Amount,
            TransactionType.Transfer,
            PaymentMethod.Other,
            "Transferência recebida",
            DateTimeOffset.UtcNow,
            accountId: request.DestinationAccountId,
            transferId: transfer.Id);

        await _transactionRepository.AddAsync(
            sourceTransaction,
            cancellationToken);

        await _transactionRepository.AddAsync(
            destinationTransaction,
            cancellationToken);

        await _transactionRepository.SaveChangesAsync(
            cancellationToken);

        return new TransferResponse(
            transfer.Id,
            transfer.SourceAccountId,
            transfer.DestinationAccountId,
            transfer.Amount,
            transfer.CreatedAt);
    }

    public async Task<TransferResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var transfer = await _transferRepository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (transfer is null)
            return null;

        return new TransferResponse(
            transfer.Id,
            transfer.SourceAccountId,
            transfer.DestinationAccountId,
            transfer.Amount,
            transfer.CreatedAt);
    }

    public async Task<TransferResponse> UpdateAsync(
        int id,
        UpdateTransferRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var transfer = await _transferRepository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (transfer is null)
        {
            throw new KeyNotFoundException(
                "Transfer not found.");
        }

        if (request.SourceAccountId == request.DestinationAccountId)
        {
            throw new ArgumentException(
                "Source and destination accounts must be different.",
                nameof(request.DestinationAccountId));
        }

        if (request.Amount <= 0)
        {
            throw new ArgumentException(
                "Transfer amount must be greater than zero.",
                nameof(request.Amount));
        }

        var transactions = await _transactionRepository
            .GetByTransferIdAsync(
                transfer.Id,
                userId,
                cancellationToken);

        if (transactions.Count != 2)
        {
            throw new InvalidOperationException(
                "Transfer must have exactly two transactions.");
        }

        var sourceTransaction = transactions.FirstOrDefault(
            transaction =>
                transaction.AccountId == transfer.SourceAccountId);

        var destinationTransaction = transactions.FirstOrDefault(
            transaction =>
                transaction.AccountId == transfer.DestinationAccountId);

        if (sourceTransaction is null ||
            destinationTransaction is null)
        {
            throw new InvalidOperationException(
                "Transfer transactions are invalid.");
        }

        var oldSourceAccount = await _accountRepository.GetByIdAsync(
            transfer.SourceAccountId,
            userId,
            cancellationToken);

        if (oldSourceAccount is null)
        {
            throw new ArgumentException(
                "Source account not found.",
                nameof(transfer.SourceAccountId));
        }

        var oldDestinationAccount = await _accountRepository.GetByIdAsync(
            transfer.DestinationAccountId,
            userId,
            cancellationToken);

        if (oldDestinationAccount is null)
        {
            throw new ArgumentException(
                "Destination account not found.",
                nameof(transfer.DestinationAccountId));
        }

        var newSourceAccount = await _accountRepository.GetByIdAsync(
            request.SourceAccountId,
            userId,
            cancellationToken);

        if (newSourceAccount is null)
        {
            throw new ArgumentException(
                "Source account not found.",
                nameof(request.SourceAccountId));
        }

        if (!newSourceAccount.IsActive)
        {
            throw new ArgumentException(
                "Source account is inactive.",
                nameof(request.SourceAccountId));
        }

        var newDestinationAccount = await _accountRepository.GetByIdAsync(
            request.DestinationAccountId,
            userId,
            cancellationToken);

        if (newDestinationAccount is null)
        {
            throw new ArgumentException(
                "Destination account not found.",
                nameof(request.DestinationAccountId));
        }

        if (!newDestinationAccount.IsActive)
        {
            throw new ArgumentException(
                "Destination account is inactive.",
                nameof(request.DestinationAccountId));
        }

        oldSourceAccount.Credit(transfer.Amount);
        oldDestinationAccount.Debit(transfer.Amount);

        if (newSourceAccount.CurrentBalance < request.Amount)
        {
            throw new InvalidOperationException(
                "Insufficient funds.");
        }

        newSourceAccount.Debit(request.Amount);
        newDestinationAccount.Credit(request.Amount);

        transfer.Update(
            request.SourceAccountId,
            request.DestinationAccountId,
            request.Amount);

        sourceTransaction.UpdateTransfer(
            request.Amount,
            request.SourceAccountId);

        destinationTransaction.UpdateTransfer(
            request.Amount,
            request.DestinationAccountId);

        await _transferRepository.SaveChangesAsync(
            cancellationToken);

        return new TransferResponse(
            transfer.Id,
            transfer.SourceAccountId,
            transfer.DestinationAccountId,
            transfer.Amount,
            transfer.CreatedAt);
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var transfer = await _transferRepository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (transfer is null)
        {
            throw new KeyNotFoundException(
                "Transfer not found.");
        }

        var sourceAccount = await _accountRepository.GetByIdAsync(
            transfer.SourceAccountId,
            userId,
            cancellationToken);

        if (sourceAccount is null)
        {
            throw new ArgumentException(
                "Source account not found.",
                nameof(transfer.SourceAccountId));
        }

        var destinationAccount = await _accountRepository.GetByIdAsync(
            transfer.DestinationAccountId,
            userId,
            cancellationToken);

        if (destinationAccount is null)
        {
            throw new ArgumentException(
                "Destination account not found.",
                nameof(transfer.DestinationAccountId));
        }

        destinationAccount.Debit(transfer.Amount);
        sourceAccount.Credit(transfer.Amount);

        await _transactionRepository.DeleteByTransferIdAsync(
            transfer.Id,
            userId,
            cancellationToken);

        await _transferRepository.DeleteAsync(
            transfer,
            cancellationToken);

        await _transferRepository.SaveChangesAsync(
            cancellationToken);
    }
}