namespace FiscalizAI.Core.Domain.Exceptions;

public class AuthException : Exception
{
    public AuthException(string message) : base(message) { }
}