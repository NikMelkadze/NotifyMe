using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using NotifyMe.Domain.Entities;
using NotifyMe.Domain.Enums;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Persistence;

namespace NotifyMe.Worker;

public class Worker(
    ILogger<Worker> logger,
    IServiceProvider serviceProvider,
    IHttpClientService httpClientService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            List<UserSavedProduct> products;
            string html;
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                products = await dbContext.UserSavedProducts.ToListAsync(stoppingToken);
            }

            foreach (var product in products)
            {
                try
                {
                    html = await httpClientService.FetchHtmlFromWeb(product.Url);
                }
                catch (Exception e)
                {
                    logger.LogInformation(e.ToString());
                    break;
                }

                var item = await httpClientService.GetPriceElements(html, product.Shop, stoppingToken);

                if (item.isDiscounted)
                {
                    string email;

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        email = (await dbContext.User
                            .Where(x => x.Id == product.UserId)
                            .Select(x => x.Email)
                            .FirstOrDefaultAsync(stoppingToken))!;
                    }

                    SendEmail(email, product.Shop, item.currentPrice, item.prevPrice);
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

    private void SendEmail(string userEmail, Shop shop, string currentPrice, string prevPrice)
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
        mail.Body = $"მიმდინარე ფასი: {currentPrice},ძველი ფასი: {prevPrice}";

        smtpClient.Send(mail);
    }
}