using System.Net;
using System.Net.Mail;
using AngleSharp;
using Microsoft.EntityFrameworkCore;
using NotifyMe.Domain.Entities;
using NotifyMe.Domain.Enums;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Services;
using NotifyMe.Persistence;

namespace NotifyMe.Worker;

public class Worker(
    ILogger<Worker> logger,
    IServiceProvider serviceProvider,
    IHttpClientService httpClientService,
    IBrowsingContext browsingContext) : BackgroundService
{
   // private readonly TimeSpan _targetTime = new(17, 28, 0);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
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
                products = await dbContext.UserSavedProducts.Where(x => x.IsActive).ToListAsync(stoppingToken);
            }

            foreach (var product in products)
            {
                (bool,string,string) discountInfo;
                try
                {
                    if (product.Shop is Shop.Alta or Shop.Megatechnica)
                    {
                        var html = await httpClientService.GetHtml(product.Url,stoppingToken);
                        var factory = new FetchDataFromHtml(browsingContext);
                        discountInfo = await factory.GetDiscountInformation(html, product.Shop, stoppingToken);
                    }

                    else
                    {
                        var response = await httpClientService.GetProductJson(product.Url, stoppingToken);
                        var factory = new FetchDataFromJson();
                        discountInfo = await factory.GetDiscountInformation(response, product.Shop, stoppingToken);
                    }
                }
                catch (Exception e)
                {
                    logger.LogInformation(e.ToString(),"Error");
                    break;
                }

                if (discountInfo.Item1)
                {
                    Console.WriteLine($"{product.Shop} - {product.Name} Item is Discounted");
                    string email;

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        email = (await dbContext.User
                            .Where(x => x.Id == product.UserId)
                            .Select(x => x.Email)
                            .FirstOrDefaultAsync(stoppingToken))!;
                    }

                    SendEmail(email, product.Name,product.Shop, discountInfo.Item2, discountInfo.Item3);
                }
                else
                {
                    Console.WriteLine($"{product.Shop} - {product.Name} Item is not Discounted");
                }
            }
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);

        }
    }

    private static void SendEmail(string userEmail,string productName, Shop shop, string currentPrice, string prevPrice)
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
}