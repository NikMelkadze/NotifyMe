using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Domain.Entities;

public class SavedProduct
{
    [Key] public int Id { get; set; }
    public string Url { get; set; } = null!;
    public string Name { get; set; } = null!;
    public NotificationType NotificationType { get; set; }
    public ProductStatus Status { get; set; }
    public int UserId { get; set; }
    public DateTime? LastNotificationSentAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public int SentNotificationCount { get; set; }
    [Precision(6, 2)] public decimal InitialPrice { get; set; }
    [Precision(6, 2)] public decimal? RegularPrice { get; set; }
    [Precision(6, 2)] public decimal? DiscountedPrice { get; set; }
    public int ShopId { get; set; }

    public Shop Shop { get; set; } = null!;
}