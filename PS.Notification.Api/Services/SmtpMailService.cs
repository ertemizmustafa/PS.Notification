using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using PS.Notification.Abstractions.Commands;
using PS.Notification.Api.Interfaces;
using PS.Notification.Api.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PS.Notification.Api.Services
{
    public class SmtpMailService : IEMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<SmtpMailService> _logger;
        public SmtpMailService(IOptions<MailSettings> mailSettings, ILogger<SmtpMailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task SendAsync(SendMailCommand command)
        {
            var email = new MimeMessage
            {
                Subject = command.Subject,
                Body = new BodyBuilder
                {
                    HtmlBody = command.Body
                }.ToMessageBody()
            };

            email.From.Add(new MailboxAddress(command.FromDisplayName, command.From));
            command.To?.ToList().ForEach(item => email.To.Add(MailboxAddress.Parse(item)));
            command.Cc?.ToList().ForEach(item => email.Cc.Add(MailboxAddress.Parse(item)));

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port);
            smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password);
            var res = await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
