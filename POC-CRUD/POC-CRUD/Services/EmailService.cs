using MailKit.Net.Smtp;
using POC_CRUD.Configurations;
using POC_CRUD.DTOs;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace POC_CRUD.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendEmail(EmailPayload payload)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_settings.From));
        email.To.Add(MailboxAddress.Parse(payload.To));
        email.Subject = payload.Subject;
        email.Body = new TextPart("html") { Text = payload.Body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}