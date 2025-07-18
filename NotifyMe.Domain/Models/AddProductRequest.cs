using NotifyMe.Domain.Enums;

namespace NotifyMe.Domain.Models;

public class AddProductRequest
{
    public string Url { get; set; } = null!;
    public NotificationTypes NotificationType { get; set; }

}