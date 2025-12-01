using Application.Interfaces;
using Infrastructure.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly ILogger<SmtpEmailService> logger;
        private readonly IOptions<SmtpOptions> options;

        public SmtpEmailService(ILogger<SmtpEmailService> logger, IOptions<SmtpOptions> options)
        {
            this.logger = logger;
            this.options = options;
        }
        public async Task SendConfirmationEmailAsync(string toEmail, string confirmationLink, CancellationToken cancellationToken)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("InnoShop", options.Value.SenderEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = "Confirm your email";

                message.Body = new TextPart("html")
                {
                    Text = $@"
                    <h2>Welcome!</h2>
                    <p>Please confirm your email adress:</p>
                    <p><a href=""{confirmationLink}"">Confirm email</a></p>"
                };

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(
                    options.Value.Host,
                    options.Value.Port,
                    options.Value.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None,
                    cancellationToken
                );

                if (!string.IsNullOrEmpty(options.Value.Username) && !string.IsNullOrEmpty(options.Value.Password))
                {
                    await smtp.AuthenticateAsync(options.Value.Username, options.Value.Password, cancellationToken);
                }

                await smtp.SendAsync(message, cancellationToken);
                await smtp.DisconnectAsync(true, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send confirmation email to {Email}", toEmail);
                throw new Exception($"Failed to send confirmation email to {toEmail}");
            }

        }
    }
}
