using Gardula.Application.Authentication.DTOs;
using Gardula.Application.Authentication.Exceptions;
using Gardula.Application.Authentication.Validation;
using Gardula.Domain.Entities.Authentication;

namespace Gardula.Application.Authentication.Services;

public class RegisterService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterResponse> ExecuteAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        if (!PasswordPolicy.IsValid(request.Password))
            throw new InvalidPasswordException();

        var emailAlreadyExists =
            await _userRepository.ExistsByEmailAsync(
                email,
                cancellationToken);

        if (emailAlreadyExists)
            throw new EmailAlreadyExistsException();

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = new User(
            request.Name.Trim(),
            email,
            passwordHash);

        await _userRepository.AddAsync(
            user,
            cancellationToken);

        return new RegisterResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}