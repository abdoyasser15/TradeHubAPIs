using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Service_Contract;
using static System.Net.WebRequestMethods;

namespace TradeHub.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration _config;

        public NotificationService(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendAsync(string to, string message, string subject = null)
        {
            if (to.Contains("@"))
                await SendEmailAsync(to, message, subject);
            else
                await SendSmsAsync(to, message);
        }
        private async Task SendEmailAsync(string to, string subject, string body)
        {
            // ✅ تأكيد إن كل القيم مش null
            to ??= string.Empty;
            subject ??= "TradeHub Notification";
            body = $"Hello {to} Thank You for signing to TradeHub";

            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(
                _config["EmailSettings:FromName"] ?? "TradeHub",
                _config["EmailSettings:FromEmail"] ?? "noreply@tradehub.com"
            ));

            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = body
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _config["EmailSettings:SmtpServer"],
                int.Parse(_config["EmailSettings:SmtpPort"]),
                MailKit.Security.SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(
                _config["EmailSettings:Username"],
                _config["EmailSettings:Password"]
            );

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        private async Task SendSmsAsync(string phoneNumber, string message)
        {
            Console.WriteLine($"SMS sent to {phoneNumber}: {message}");
            await Task.CompletedTask;
        }
    }
}
