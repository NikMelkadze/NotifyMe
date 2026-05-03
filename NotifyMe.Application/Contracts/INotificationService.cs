namespace NotifyMe.Application.Contracts;

public interface INotificationService
{
    void SendEmail(string to, string subject, string body);
}