namespace NotifyMe.Infrastructure.Models.ApiResponse;

public class ProductBase
{
    public Product Product { get; set; }
}

public struct Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public double? PreviousPrice { get; set; }
}