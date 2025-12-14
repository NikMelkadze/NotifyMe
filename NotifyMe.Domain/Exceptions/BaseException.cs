namespace NotifyMe.Domain.Exceptions;

public class BaseException : Exception
{
    protected BaseException(string message) : base(message) { }
}