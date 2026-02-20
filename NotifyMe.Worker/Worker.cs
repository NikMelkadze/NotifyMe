using System.Net;
using System.Net.Mail;
using AngleSharp;
using Microsoft.EntityFrameworkCore;
using NotifyMe.Domain.Entities;
using NotifyMe.Domain.Enums;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Extensions;
using NotifyMe.Infrastructure.Models;
using NotifyMe.Infrastructure.Services.ShopProductServices;
using NotifyMe.Persistence;

namespace NotifyMe.Worker;

public class Worker(
    ILogger<Worker> logger,
    IServiceProvider serviceProvider,
    IHttpClientService httpClientService,
    IBrowsingContext browsingContext,
    IHostApplicationLifetime lifetime) : BackgroundService
{
    // private readonly TimeSpan _targetTime = new(17, 28, 0);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // var now = DateTime.Now;
        // var nextRunTime = DateTime.Today.Add(_targetTime);
        //
        // if (now > nextRunTime)
        //     nextRunTime = nextRunTime.AddDays(1);
        //
        // var delay = nextRunTime - now;
        //
        // logger.LogInformation($"Next run at {nextRunTime}. Waiting {delay.TotalMinutes} minutes.");
        // await Task.Delay(delay, stoppingToken);

        List<UserSavedProduct> products;
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            products = await dbContext.UserSavedProducts
                .Where(x => x.IsActive && (x.LastNotificationSentAt == null ||
                                           x.LastNotificationSentAt.Value.Date != DateTime.Today.Date))
                .ToListAsync(stoppingToken);
        }

        foreach (var product in products)
        {
            ProductPriceInformation priceInformation;
            try
            {
                var html = await httpClientService.GetHtml(product.Url, stoppingToken);
                var document = await browsingContext.OpenAsync(req => req.Content(html), stoppingToken);

                var factory = new ShopProductFactory();
                var shopFactory = factory.GetShopFactory(product.Shop);

                priceInformation = shopFactory.GetPriceInformation(document);
            }
            catch (Exception e)
            {
                logger.LogInformation(e.ToString(), "Error while parsing url");
                continue;
            }

            var productNewRegularPrice = Convert.ToDecimal(priceInformation.Price.NormalizePrice());

            var hasNewRegularPrice = product.InitialPrice != productNewRegularPrice ||
                                     product.RegularPrice != productNewRegularPrice;

            if (hasNewRegularPrice)
            {
                await SaveNewRegularPrice(productNewRegularPrice, product, stoppingToken);
            }

            var discountedPrice = Convert.ToDecimal(priceInformation.DiscountedPrice?.NormalizePrice());


            if (priceInformation.IsDiscounted)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    dbContext.Attach(product);

                    product.LastNotificationSentAt = DateTime.Now;
                    product.SentNotificationCount++;

                    if (product.DiscountedPrice == null || product.DiscountedPrice != discountedPrice)
                    {
                        product.DiscountedPrice = discountedPrice;
                    }

                    var email = (await dbContext.User
                        .Where(x => x.Id == product.UserId)
                        .Select(x => x.Email)
                        .FirstOrDefaultAsync(stoppingToken))!;

                    SendEmail(email, product.Name, product.Shop, priceInformation.DiscountedPrice!,
                        priceInformation.Price);

                    await dbContext.SaveChangesAsync(stoppingToken);
                }
            }
            else
            {
                if (product.DiscountedPrice != null)
                {
                    await RemoveDiscountPrice(product, stoppingToken);
                    Console.WriteLine($"{product.Shop} - {product.Name} no longer has discount");
                }

                Console.WriteLine($"{product.Shop} - {product.Name} Item is not Discounted");
            }
        }

        lifetime.StopApplication();
        //  await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
    }

    private static void SendEmail(string userEmail, string productName, Shop shop, string currentPrice,
        string prevPrice)
    {
        var mail = new MailMessage
        {
            From = new MailAddress("notifymeinformation@gmail.com")
        };

        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential("notifymeinformation@gmail.com", "urog lnsb zjkl wbtn "),
            EnableSsl = true
        };

        mail.To.Add(userEmail);
        mail.Subject = $"{shop} - ის ფასდაკლება მოთხოვნილ პროდუქტზე";

        mail.Body = $"დასახელება:{productName},\n" +
                    $"მიმდინარე ფასი: {currentPrice},\n" +
                    $"ძველი ფასი: {prevPrice}";

        smtpClient.Send(mail);
        Console.WriteLine($"Email sent to {userEmail}");
    }

    private async Task SaveNewRegularPrice(decimal newRegularPrice, UserSavedProduct product,
        CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Attach(product);

        product.RegularPrice = newRegularPrice;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task RemoveDiscountPrice(UserSavedProduct product,
        CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Attach(product);

        product.DiscountedPrice = null;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}