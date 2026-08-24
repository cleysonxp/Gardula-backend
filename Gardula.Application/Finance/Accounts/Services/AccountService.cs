using Gardula.Application.Common.Services;
using Gardula.Application.Finance.Accounts.DTOs;
using Gardula.Domain.Entities.Finance;
using Gardula.Application.Finance.Transactions.DTOs;
using Gardula.Application.Finance.Transactions.Services;

namespace Gardula.Application.Finance.Accounts.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly TransactionService _transactionService;

    public AccountService(
        IAccountRepository accountRepository,
        ICurrentUserService currentUserService,
        TransactionService transactionService)
    {
        _accountRepository = accountRepository;
        _currentUserService = currentUserService;
        _transactionService = transactionService;
    }

    public async Task<AccountResponse> CreateAsync(
        CreateAccountRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var account = new Account(
            userId,
            request.Name.Trim(),
            (AccountType)request.Type,
            request.InitialBalance,
            request.Color.Trim());

        await _accountRepository.AddAsync(
            account,
            cancellationToken);

        return new AccountResponse(
            account.Id,
            account.Name,
            (int)account.Type,
            account.InitialBalance,
            account.Color,
            account.IsActive,
            account.CreatedAt,
            account.UpdatedAt);
    }

    public async Task<List<AccountResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var accounts = await _accountRepository.GetAllByUserIdAsync(
            userId,
            cancellationToken);

        return accounts
            .Select(account => new AccountResponse(
                account.Id,
                account.Name,
                (int)account.Type,
                account.InitialBalance,
                account.Color,
                account.IsActive,
                account.CreatedAt,
                account.UpdatedAt))
            .ToList();
    }

    public async Task<AccountResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var account = await _accountRepository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (account is null)
            return null;

        return new AccountResponse(
            account.Id,
            account.Name,
            (int)account.Type,
            account.InitialBalance,
            account.Color,
            account.IsActive,
            account.CreatedAt,
            account.UpdatedAt);
    }

    public async Task<AccountResponse?> UpdateAsync(
        int id,
        UpdateAccountRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var account = await _accountRepository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (account is null)
            return null;

        account.Update(
            request.Name.Trim(),
            (AccountType)request.Type);

        await _accountRepository.SaveChangesAsync(
            cancellationToken);

        return new AccountResponse(
            account.Id,
            account.Name,
            (int)account.Type,
            account.InitialBalance,
            account.Color,
            account.IsActive,
            account.CreatedAt,
            account.UpdatedAt);
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var account = await _accountRepository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (account is null)
            return false;

        account.Deactivate();

        await _accountRepository.SaveChangesAsync(
            cancellationToken);

        return true;
    }

    public async Task<AccountOverviewResponse> GetOverviewAsync(
        AccountOverviewFilterRequest filter,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var startDate = filter.StartDate
            ?? new DateTimeOffset(
                DateTimeOffset.UtcNow.Year,
                DateTimeOffset.UtcNow.Month,
                1,
                0,
                0,
                0,
                TimeSpan.Zero);

        var endDate = filter.EndDate
            ?? startDate.AddMonths(1).AddTicks(-1);

        var accounts = await _accountRepository.GetAllByUserIdAsync(
            userId,
            cancellationToken);

        var balances = await _accountRepository.GetBalancesByUserIdAsync(
            userId,
            cancellationToken);

        var periodSummaries =
            await _accountRepository.GetPeriodSummaryByUserIdAsync(
                userId,
                startDate,
                endDate,
                cancellationToken);

        var recentTransactions =
            await _transactionService.GetAllAsync(
                new TransactionFilterRequest(
                    StartDate: startDate,
                    EndDate: endDate,
                    Page: 1,
                    PageSize: 5),
                cancellationToken);

        var balanceByAccountId = balances
            .ToDictionary(
                item => item.AccountId,
                item => item.CurrentBalance);

        var summaryByAccountId = periodSummaries
            .ToDictionary(
                item => item.AccountId,
                item => item);

        var accountItems = accounts
            .Select(account =>
            {
                balanceByAccountId.TryGetValue(
                    account.Id,
                    out var currentBalance);

                summaryByAccountId.TryGetValue(
                    account.Id,
                    out var summary);

                return new AccountOverviewItem(
                    account.Id,
                    account.Name,
                    (int)account.Type,
                    account.Color,
                    account.IsActive,
                    currentBalance,
                    summary?.Income ?? 0m,
                    summary?.Expense ?? 0m);
            })
            .ToList();

        return new AccountOverviewResponse(
            accountItems.Sum(account => account.CurrentBalance),
            accounts.Count,
            accountItems.Sum(account => account.Income),
            accountItems.Sum(account => account.Expense),
            accountItems,
            recentTransactions.Items);
    }

    public async Task<AccountDetailOverviewResponse?> GetDetailOverviewAsync(
        int id,
        AccountDetailFilterRequest filter,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var account = await _accountRepository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (account is null)
            return null;

        var currentBalance =
            await _accountRepository.GetBalanceByAccountIdAsync(
                id,
                userId,
                cancellationToken);

        var transactionFilter = new TransactionFilterRequest(
            StartDate: filter.StartDate,
            EndDate: filter.EndDate,
            CategoryId: filter.CategoryId,
            AccountId: id,
            Search: filter.Search,
            Page: filter.Page,
            PageSize: filter.PageSize);

        var summary = await _transactionService.GetSummaryAsync(
            transactionFilter,
            cancellationToken);

        var transactions = await _transactionService.GetAllAsync(
            transactionFilter,
            cancellationToken);

        var accountResponse = new AccountResponse(
            account.Id,
            account.Name,
            (int)account.Type,
            account.InitialBalance,
            account.Color,
            account.IsActive,
            account.CreatedAt,
            account.UpdatedAt);

        return new AccountDetailOverviewResponse(
            accountResponse,
            currentBalance,
            summary,
            transactions);
    }

}