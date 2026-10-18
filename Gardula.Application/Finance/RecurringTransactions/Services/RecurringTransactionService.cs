using Gardula.Application.Common.Services;
using Gardula.Application.Finance.Accounts.Services;
using Gardula.Application.Finance.Cards.Services;
using Gardula.Application.Finance.Categories.Services;
using Gardula.Application.Finance.RecurringTransactions.DTOs;
using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.RecurringTransactions.Services;

public class RecurringTransactionService
{
    private readonly IRecurringTransactionRepository _repository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICardRepository _cardRepository;
    private readonly ICurrentUserService _currentUserService;

    public RecurringTransactionService(
        IRecurringTransactionRepository repository,
        ICategoryRepository categoryRepository,
        IAccountRepository accountRepository,
        ICardRepository cardRepository,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _accountRepository = accountRepository;
        _cardRepository = cardRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<RecurringTransactionResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var recurringTransactions = await _repository.GetAllByUserIdAsync(
            userId,
            cancellationToken);

        var responses = new List<RecurringTransactionResponse>();

        foreach (var recurringTransaction in recurringTransactions)
        {
            responses.Add(await MapToResponseAsync(
                recurringTransaction,
                cancellationToken));
        }

        return responses;
    }

    public async Task<RecurringTransactionResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var recurringTransaction = await _repository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (recurringTransaction is null)
            return null;

        return await MapToResponseAsync(
            recurringTransaction,
            cancellationToken);
    }

    public async Task<RecurringTransactionResponse> CreateAsync(
        CreateRecurringTransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        ValidateRequestEnums(
            request.Type,
            request.PaymentMethod);

        await ValidateReferencesAsync(
            userId,
            (TransactionType)request.Type,
            request.CategoryId,
            (PaymentMethod)request.PaymentMethod,
            request.AccountId,
            request.CardId,
            cancellationToken);

        var recurringTransaction = new RecurringTransaction(
            userId,
            request.Name,
            (TransactionType)request.Type,
            request.Amount,
            request.CategoryId,
            (PaymentMethod)request.PaymentMethod,
            request.AccountId,
            request.CardId,
            request.DueDay,
            request.StartDate,
            request.EndDate);

        await _repository.AddAsync(
            recurringTransaction,
            cancellationToken);

        return await MapToResponseAsync(
            recurringTransaction,
            cancellationToken);
    }

    public async Task<RecurringTransactionResponse?> UpdateAsync(
        int id,
        UpdateRecurringTransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var recurringTransaction = await _repository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (recurringTransaction is null)
            return null;

        ValidateRequestEnums(
            request.Type,
            request.PaymentMethod);

        await ValidateReferencesAsync(
            userId,
            (TransactionType)request.Type,
            request.CategoryId,
            (PaymentMethod)request.PaymentMethod,
            request.AccountId,
            request.CardId,
            cancellationToken);

        recurringTransaction.Update(
            request.Name,
            (TransactionType)request.Type,
            request.Amount,
            request.CategoryId,
            (PaymentMethod)request.PaymentMethod,
            request.AccountId,
            request.CardId,
            request.DueDay,
            request.StartDate,
            request.EndDate);

        await _repository.SaveChangesAsync(cancellationToken);

        return await MapToResponseAsync(
            recurringTransaction,
            cancellationToken);
    }

    public async Task<bool> DeactivateAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var recurringTransaction = await _repository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (recurringTransaction is null)
            return false;

        recurringTransaction.Deactivate();

        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ActivateAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var recurringTransaction = await _repository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (recurringTransaction is null)
            return false;

        recurringTransaction.Activate();

        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var recurringTransaction = await _repository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (recurringTransaction is null)
            return false;

        await _repository.DeleteAsync(
            recurringTransaction,
            cancellationToken);

        return true;
    }

    private async Task ValidateReferencesAsync(
        int userId,
        TransactionType type,
        int categoryId,
        PaymentMethod paymentMethod,
        int? accountId,
        int? cardId,
        CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(
            categoryId,
            userId,
            cancellationToken);

        if (category is null)
        {
            throw new ArgumentException(
                "Category not found or inactive.",
                nameof(categoryId));
        }

        var expectedCategoryType = type == TransactionType.Income
            ? CategoryType.Income
            : CategoryType.Expense;

        if (category.Type != expectedCategoryType)
        {
            throw new ArgumentException(
                "Category type must match the recurring transaction type.",
                nameof(categoryId));
        }

        if (paymentMethod == PaymentMethod.CreditCard)
        {
            if (!cardId.HasValue || accountId.HasValue)
            {
                throw new ArgumentException(
                    "Credit card payments require CardId and must not include AccountId.");
            }

            var card = await _cardRepository.GetByIdAsync(
                cardId.Value,
                userId,
                cancellationToken);

            if (card is null || !card.IsActive)
            {
                throw new ArgumentException(
                    "Card not found or inactive.",
                    nameof(cardId));
            }

            return;
        }

        if (!accountId.HasValue || cardId.HasValue)
        {
            throw new ArgumentException(
                "This payment method requires AccountId and must not include CardId.");
        }

        var account = await _accountRepository.GetByIdAsync(
            accountId.Value,
            userId,
            cancellationToken);

        if (account is null || !account.IsActive)
        {
            throw new ArgumentException(
                "Account not found or inactive.",
                nameof(accountId));
        }
    }

    private static void ValidateRequestEnums(
        int typeValue,
        int paymentMethodValue)
    {
        var type = (TransactionType)typeValue;

        if (type is not TransactionType.Income
            and not TransactionType.Expense)
        {
            throw new ArgumentException(
                "Recurring transaction type must be Income or Expense.",
                nameof(typeValue));
        }

        var paymentMethod = (PaymentMethod)paymentMethodValue;

        if (!Enum.IsDefined(paymentMethod))
        {
            throw new ArgumentException(
                "Invalid payment method.",
                nameof(paymentMethodValue));
        }
    }

    private async Task<RecurringTransactionResponse> MapToResponseAsync(
        RecurringTransaction recurringTransaction,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var category = await _categoryRepository.GetByIdAsync(
            recurringTransaction.CategoryId,
            userId,
            cancellationToken);

        string? accountName = null;
        string? cardName = null;

        if (recurringTransaction.CardId.HasValue)
        {
            var card = await _cardRepository.GetByIdAsync(
                recurringTransaction.CardId.Value,
                userId,
                cancellationToken);

            cardName = card?.Name;
        }
        else if (recurringTransaction.AccountId.HasValue)
        {
            var account = await _accountRepository.GetByIdAsync(
                recurringTransaction.AccountId.Value,
                userId,
                cancellationToken);

            accountName = account?.Name;
        }

        return new RecurringTransactionResponse(
            recurringTransaction.Id,
            recurringTransaction.Name,
            (int)recurringTransaction.Type,
            recurringTransaction.Amount,
            recurringTransaction.CategoryId,
            category?.Name ?? string.Empty,
            (int)recurringTransaction.PaymentMethod,
            recurringTransaction.AccountId,
            accountName,
            recurringTransaction.CardId,
            cardName,
            recurringTransaction.DueDay,
            recurringTransaction.StartDate,
            recurringTransaction.EndDate,
            recurringTransaction.IsActive,
            recurringTransaction.CreatedAt,
            recurringTransaction.UpdatedAt);
    }
}
