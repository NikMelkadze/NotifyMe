namespace NotifyMe.Application.Models.UserProducts;

public class UserSavedProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string NotificationType { get; set; } = null!;
    public bool IsActive { get; set; }
    public string Shop { get; set; } = null!;
    public string Url { get; set; } = null!;
    public decimal InitialPrice { get; set; }
    public decimal? NewPrice { get; set; }
    public string? DiscountPercentage { get; set; }
    public decimal? PriceDifference { get; set; }
}