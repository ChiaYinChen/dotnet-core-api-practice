using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using WebApiApp.Constants;
using WebApiApp.Helpers;
using WebApiApp.Models;

namespace WebApiApp.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly TemplateService _templateService;

        public EmailService(
            IOptions<EmailSettings> emailSettings,
            TemplateService templateService,
        )
        {
            _emailSettings = emailSettings.Value;
            _templateService = templateService;
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

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception exc)
            {
                throw new BadRequestError(CustomErrorCode.InvalidOperation, "Send email failed");
            }
        }

        public async Task SendRegisterSuccessEmail(string email, string userName)
        {
            var htmlContent = _templateService.RenderTemplate("register_success.html", new { user_name = userName });
            await Send(
                receivers: [email],
                subject: "Sign-up Successful",
                body: htmlContent
            );
        }
    }
}
