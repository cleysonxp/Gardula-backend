using Gardula.Application.Common.Services;
using Gardula.Application.Finance.Accounts.Services;
using Gardula.Application.Finance.Cards.Services;
using Gardula.Application.Finance.Categories.Services;
using Gardula.Application.Finance.Transactions.DTOs;
using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Transactions.Services;

public class TransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICardRepository _cardRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUserService;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository,
        ICardRepository cardRepository,
        ICategoryRepository categoryRepository,
        ICurrentUserService currentUserService)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _cardRepository = cardRepository;
        _categoryRepository = categoryRepository;
        _currentUserService = currentUserService;
    }

    public async Task<TransactionResponse> CreateAsync(
        CreateTransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var type = (TransactionType)request.Type;

        if (!Enum.IsDefined(type))
            throw new ArgumentException(
                "Invalid transaction type.",
                nameof(request.Type));

        var paymentMethod = (PaymentMethod)request.PaymentMethod;

        if (!Enum.IsDefined(paymentMethod))
            throw new ArgumentException(
                "Invalid payment method.",
                nameof(request.PaymentMethod));

        if (type == TransactionType.Transfer)
        {
            throw new ArgumentException(
                "Transfers must be created through the transfer flow.",
                nameof(request.Type));
        }

        if (!request.CategoryId.HasValue)
        {
            throw new ArgumentException(
                "CategoryId is required.",
                nameof(request.CategoryId));
        }

        var category = await _categoryRepository.GetByIdAsync(
            request.CategoryId.Value,
            userId,
            cancellationToken);

        if (category is null)
        {
            throw new ArgumentException(
                "Category not found.",
                nameof(request.CategoryId));
        }

        if (category.Type == CategoryType.Income &&
            type != TransactionType.Income)
        {
            throw new ArgumentException(
                "Income category cannot be used for an expense.",
                nameof(request.CategoryId));
        }

        if (category.Type == CategoryType.Expense &&
            type != TransactionType.Expense)
        {
            throw new ArgumentException(
                "Expense category cannot be used for an income.",
                nameof(request.CategoryId));
        }

        if (paymentMethod == PaymentMethod.CreditCard)
        {
            if (!request.CardId.HasValue)
            {
                throw new ArgumentException(
                    "CardId is required for credit card transactions.",
                    nameof(request.CardId));
            }

            if (request.AccountId.HasValue)
            {
                throw new ArgumentException(
                    "AccountId cannot be used with credit card transactions.",
                    nameof(request.AccountId));
            }

            var card = await _cardRepository.GetByIdAsync(
                request.CardId.Value,
                userId,
                cancellationToken);

            if (card is null)
            {
                throw new ArgumentException(
                    "Card not found.",
                    nameof(request.CardId));
            }
        }
        else
        {
            if (!request.AccountId.HasValue)
            {
                throw new ArgumentException(
                    "AccountId is required for this payment method.",
                    nameof(request.AccountId));
            }

            if (request.CardId.HasValue)
            {
                throw new ArgumentException(
                    "CardId can only be used with credit card transactions.",
                    nameof(request.CardId));
            }

            var account = await _accountRepository.GetByIdAsync(
                request.AccountId.Value,
                userId,
                cancellationToken);

            if (account is null)
            {
                throw new ArgumentException(
                    "Account not found.",
                    nameof(request.AccountId));
            }

            if (!account.IsActive)
            {
                throw new ArgumentException(
                    "Account is inactive.",
                    nameof(request.AccountId));
            }
        }

        var totalInstallments = request.TotalInstallments ?? 1;

        if (totalInstallments <= 0)
        {
            throw new ArgumentException(
                "TotalInstallments must be greater than zero.",
                nameof(request.TotalInstallments));
        }

        // ============================================================
        // TRANSAÇÃO PARCELADA
        // ============================================================

        if (totalInstallments > 1)
        {
            if (type != TransactionType.Expense)
            {
                throw new ArgumentException(
                    "Only expense transactions can be split into installments.",
                    nameof(request.TotalInstallments));
            }

            if (paymentMethod != PaymentMethod.CreditCard)
            {
                throw new ArgumentException(
                    "Installments are only available for credit card transactions.",
                    nameof(request.TotalInstallments));
            }

            if (request.InstallmentGroupId.HasValue)
            {
                throw new ArgumentException(
                    "InstallmentGroupId is generated automatically.",
                    nameof(request.InstallmentGroupId));
            }

            if (request.InstallmentNumber.HasValue)
            {
                throw new ArgumentException(
                    "InstallmentNumber is generated automatically.",
                    nameof(request.InstallmentNumber));
            }

            var installmentGroupId = Guid.NewGuid();

            var installmentAmount = Math.Round(
                request.Amount / totalInstallments,
                2,
                MidpointRounding.AwayFromZero);

            Transaction? firstTransaction = null;

            for (var installmentNumber = 1;
                 installmentNumber <= totalInstallments;
                 installmentNumber++)
            {
                var amount = installmentNumber == totalInstallments
                    ? request.Amount -
                      (installmentAmount * (totalInstallments - 1))
                    : installmentAmount;

                var installmentDate = request.Date.AddMonths(
                    installmentNumber - 1);

                var transaction = new Transaction(
                    userId,
                    amount,
                    type,
                    paymentMethod,
                    request.Description.Trim(),
                    installmentDate,
                    request.AccountId,
                    request.CardId,
                    request.CategoryId,
                    null,
                    installmentGroupId,
                    installmentNumber,
                    totalInstallments);

                await _transactionRepository.AddAsync(
                    transaction,
                    cancellationToken);

                firstTransaction ??= transaction;
            }

            await _transactionRepository.SaveChangesAsync(
                cancellationToken);

            return MapToResponse(firstTransaction!);
        }

        // ============================================================
        // TRANSAÇÃO NORMAL
        // ============================================================

        if (request.InstallmentGroupId.HasValue ||
            request.InstallmentNumber.HasValue)
        {
            throw new ArgumentException(
                "Installment information is only allowed for installment transactions.");
        }

        var transactionNormal = new Transaction(
            userId,
            request.Amount,
            type,
            paymentMethod,
            request.Description.Trim(),
            request.Date,
            request.AccountId,
            request.CardId,
            request.CategoryId,
            null,
            null,
            null,
            null);

        await _transactionRepository.AddAsync(
            transactionNormal,
            cancellationToken);

        await _transactionRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(transactionNormal);
    }

    public async Task<List<TransactionResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var transactions =
            await _transactionRepository.GetAllByUserIdAsync(
                userId,
                cancellationToken);

        return transactions
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<TransactionResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var transaction =
            await _transactionRepository.GetByIdAsync(
                id,
                userId,
                cancellationToken);

        if (transaction is null)
            return null;

        return MapToResponse(transaction);
    }

    private static TransactionResponse MapToResponse(
        Transaction transaction)
    {
        return new TransactionResponse(
            transaction.Id,
            transaction.AccountId,
            transaction.CardId,
            transaction.CategoryId,
            transaction.TransferId,
            transaction.Amount,
            (int)transaction.Type,
            (int)transaction.PaymentMethod,
            transaction.Description,
            transaction.Date,
            transaction.InstallmentGroupId,
            transaction.InstallmentNumber,
            transaction.TotalInstallments,
            transaction.CreatedAt,
            transaction.UpdatedAt);
    }
}