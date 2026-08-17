using Gardula.Application.Common.Services;
using Gardula.Application.Finance.Accounts.DTOs;
using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Accounts.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentUserService _currentUserService;

    public AccountService(
        IAccountRepository accountRepository,
        ICurrentUserService currentUserService)
    {
        _accountRepository = accountRepository;
        _currentUserService = currentUserService;
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
            request.InitialBalance);

        await _accountRepository.AddAsync(
            account,
            cancellationToken);

        return new AccountResponse(
            account.Id,
            account.Name,
            (int)account.Type,
            account.InitialBalance,
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
                account.IsActive,
                account.CreatedAt,
                account.UpdatedAt))
            .ToList();
    }

}