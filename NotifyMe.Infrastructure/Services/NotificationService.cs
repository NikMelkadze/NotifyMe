using System.Net;
using System.Net.Mail;
using NotifyMe.Application.Contracts;

namespace NotifyMe.Infrastructure.Services;

public class NotificationService : INotificationService
{
    public void SendEmail(string to, string subject, string body)
    {
        var mail = new MailMessage
        {
            From = new MailAddress("notifymeinformation@gmail.com")
        };

        var smtpClient = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential("notifymeinformation@gmail.com", "urog lnsb zjkl wbtn "),
            EnableSsl = true
        };

        mail.To.Add(to);
        mail.Subject = subject;
        mail.Body = body;
        smtpClient.Send(mail);
    }
}