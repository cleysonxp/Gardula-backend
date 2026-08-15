namespace Gardula.Application.Authentication.Exceptions;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException()
        : base("The password does not meet the required security criteria.")
    {
    }
}