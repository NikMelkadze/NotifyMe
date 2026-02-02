using System.ComponentModel.DataAnnotations;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Domain.Entities;

public class UserSavedProduct
{
    [Key] public int Id { get; set; }
    public string Url { get; set; } = null!;
    public string Name { get; set; } = null!;
    public NotificationType NotificationType { get; set; }
    public bool IsActive { get; set; }
    public int UserId { get; set; }
    public Shop Shop { get; set; }
    public DateTime LastNotificationSentAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public int SentNotificationCount { get; set; }
    public double InitialPrice { get; set; }
    public double? NewPrice { get; set; }
}