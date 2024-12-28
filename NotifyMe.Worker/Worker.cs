using AngleSharp;
using AngleSharp.Dom;
using Microsoft.EntityFrameworkCore;
using NotifyMe.Domain.Entities;
using NotifyMe.Domain.Enums;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Persistence;
using Configuration = AngleSharp.Configuration;

namespace NotifyMe.Worker;

public class Worker(
    ILogger<Worker> logger,
    IServiceProvider serviceProvider,
    IHttpClientService httpClientService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = Configuration.Default;
        var context = BrowsingContext.New(config);
        while (!stoppingToken.IsCancellationRequested)
        {
            List<UserSavedProduct> products;
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                products = await dbContext.UserSavedProducts.ToListAsync(stoppingToken);
            }

            foreach (var product in products)
            {
                var html = await httpClientService.FetchHtmlFromWeb(product.Url);
                var document = await context.OpenAsync(req => req.Content(html), stoppingToken);

                var item = GetPriceElement(document, product.Shop);

                if (item.isDiscounted)
                {
                    Console.WriteLine(
                        $"{product.Shop} Item is discounted current price is: {item.currentPrice} previous price is: {item.prevPrice}");
                }
                else
                {
                    Console.WriteLine($"{product.Shop} Item is not Discounted");
                }
            }

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(5000, stoppingToken);
        }
    }

    private (bool isDiscounted, string currentPrice, string prevPrice) GetPriceElement(IDocument document, Shops shop)
    {
        if (shop == Shops.Megatechnica)
        {
            var pricesDivMega = document.QuerySelector("div.prices");
            var prevPrice = pricesDivMega!.QuerySelector("span.prev_price")?.TextContent.Trim() ?? "";
            var prevPriceTrimmed = System.Text.RegularExpressions.Regex.Replace(prevPrice, @"[^\d]", "");
            var currentPrice = pricesDivMega!.QuerySelector("span.price")?.TextContent.Trim() ?? "";
            var isDiscounted = prevPrice != "";

            return (isDiscounted, currentPrice, prevPriceTrimmed);
        }

        if (shop == Shops.Alta)
        {
            var currentAlta = document.QuerySelector(".ty-price-num")?.TextContent.Trim() ?? "";
            var prevPriceAlta = document.QuerySelector(".ty-list-price.ty-nowrap")?.TextContent ?? "";
            var prevPriceTrimmed = System.Text.RegularExpressions.Regex.Replace(prevPriceAlta, @"[^\d]", "");
            var isDiscounted = prevPriceAlta != "";

            return (isDiscounted, currentAlta, prevPriceTrimmed);
        }

        if (shop==Shops.Ee)
        {
            
        }
        throw new NotImplementedException();
    }
}