using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using WebApiApp.Models;

namespace WebApiApp.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task Send(List<string> receivers, string subject, string body)
        {
            var smtpClient = new SmtpClient(_emailSettings.HOST, _emailSettings.PORT)
            {
                Credentials = new NetworkCredential(_emailSettings.USERNAME, _emailSettings.PASSWORD),
                EnableSsl = _emailSettings.ENABLE_SSL
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.FROM_EMAIL, _emailSettings.EMAIL_SENDER),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            foreach (var receiver in receivers)
            {
                mailMessage.To.Add(receiver);
            }

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
