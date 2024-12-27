using AngleSharp;
using AngleSharp.Dom;
using Microsoft.EntityFrameworkCore;
using NotifyMe.Domain.Entities;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Persistence;
using Configuration = AngleSharp.Configuration;

namespace NotifyMe.Worker;

public class Worker(
    ILogger<Worker> logger,
    IServiceProvider  serviceProvider ,
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
                var shop = product.Shop;

                var item = GetPriceElement(document);

                if (item.isDiscounted)
                {
                    Console.WriteLine(
                        $"Item is discounted current price is: {item.currentPrice} previous price is: {item.prevPrice}");
                }
                else
                {
                    Console.WriteLine("Item is not Discounted");
                }
            }

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(5000, stoppingToken);
        }
    }

    private (bool isDiscounted, string currentPrice, string prevPrice) GetPriceElement(IDocument document)
    {
        //Megatechnica
        
        var pricesDivMega = document.QuerySelector("div.prices");
        var prevPrice = pricesDivMega!.QuerySelector("span.prev_price");
        var currentPrice = pricesDivMega!.QuerySelector("span.price");

        //alta
        var currentAlta = document.QuerySelector(".ty-price-num");
        var priceDivAlta = document.QuerySelector(".ty-list-price.ty-nowrap");

        var priceText = priceDivAlta.TextContent;
        var cleanPrice = System.Text.RegularExpressions.Regex.Replace(priceText, @"[^\d]", "");
        
        var isDiscounted = prevPrice != null;
        return (isDiscounted, currentPrice?.TextContent.Trim() ?? "", prevPrice?.TextContent.Trim() ?? "");
    }
}