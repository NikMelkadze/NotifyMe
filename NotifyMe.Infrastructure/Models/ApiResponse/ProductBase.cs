namespace NotifyMe.Infrastructure.Models.ApiResponse;

public class ProductBase
{
    public Product Product { get; set; }
}

public struct Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public decimal? PreviousPrice { get; set; }
}