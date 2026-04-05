namespace NotifyMe.Infrastructure.Models.ApiResponse;

public class EeResponse
{
    public EeResponseProduct Product { get; set; } = null!;
}

public class EeResponseProduct
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal? PreviousPrice { get; set; }
}