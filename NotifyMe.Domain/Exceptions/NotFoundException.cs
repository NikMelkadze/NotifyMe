namespace NotifyMe.Domain.Exceptions;

public sealed class NotFoundException(string message) : BaseException(message);