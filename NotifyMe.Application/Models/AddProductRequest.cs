using NotifyMe.Domain.Enums;

namespace NotifyMe.Application.Models;

public class AddProductRequest
{
    public string Url { get; set; } = null!;
    public NotificationType NotificationType { get; set; }

}