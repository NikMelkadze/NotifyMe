using NotifyMe.Domain.Enums;

namespace NotifyMe.Application.Models.UserProducts;

public class UserSavedProductsResponse
{
    public IEnumerable<Product> Products { get; set; } = null!;

    public int ActiveProductsCount { get; set; }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string NotificationType { get; set; } = null!;
    public ProductStatus Status { get; set; }
    public string Shop { get; set; } = null!;
    public string Url { get; set; } = null!;
    public decimal InitialPrice { get; set; }
    public decimal? NewPrice { get; set; }
    public decimal? DiscountedPrice { get; set; }
    public string? DiscountPercentage { get; set; }
    public decimal? PriceDifference { get; set; }
}