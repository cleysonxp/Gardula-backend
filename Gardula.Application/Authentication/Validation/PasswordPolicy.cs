namespace Gardula.Application.Authentication.Validation;

public static class PasswordPolicy
{
    public static bool IsValid(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        if (password.Length < 8)
            return false;

        if (!password.Any(char.IsUpper))
            return false;

        if (!password.Any(char.IsLower))
            return false;

        if (!password.Any(IsSpecialCharacter))
            return false;

        return true;
    }

    private static bool IsSpecialCharacter(char character)
    {
        return !char.IsLetterOrDigit(character);
    }
}