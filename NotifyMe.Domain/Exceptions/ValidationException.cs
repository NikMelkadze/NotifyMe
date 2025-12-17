namespace NotifyMe.Domain.Exceptions;

public sealed class ValidationException(string message) : BaseException(message);