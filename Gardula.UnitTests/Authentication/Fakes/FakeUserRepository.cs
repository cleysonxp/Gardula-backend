using Gardula.Application.Authentication.Services;
using Gardula.Domain.Entities.Authentication;

namespace Gardula.UnitTests.Authentication.Fakes;

public class FakeUserRepository : IUserRepository
{
    private readonly List<User> _users = [];

    public IReadOnlyCollection<User> Users => _users;

    public Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var exists = _users.Any(
            user => user.Email == email);

        return Task.FromResult(exists);
    }

    public Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var user = _users.FirstOrDefault(
            user => user.Email == email);

        return Task.FromResult(user);
    }

    public Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        _users.Add(user);

        return Task.CompletedTask;
    }
}