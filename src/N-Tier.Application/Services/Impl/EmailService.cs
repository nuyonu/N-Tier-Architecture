using MailKit.Net.Smtp;
using MimeKit;
using N_Tier.Application.Common.Email;

namespace N_Tier.Application.Services.Impl;

public class EmailService(SmtpSettings smtpSettings) : IEmailService
{
    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        await SendAsync(CreateEmail(emailMessage));
    }

    private async Task SendAsync(MimeMessage message)
    {
        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(smtpSettings.Server, smtpSettings.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(smtpSettings.Username, smtpSettings.Password);

            await client.SendAsync(message);
        }
        catch
        {
            await client.DisconnectAsync(true);
            client.Dispose();

            throw;
        }
    }

    private MimeMessage CreateEmail(EmailMessage emailMessage)
    {
        var builder = new BodyBuilder { HtmlBody = emailMessage.Body };

        if (emailMessage.Attachments.Count > 0)
            foreach (var attachment in emailMessage.Attachments)
                builder.Attachments.Add(attachment.Name, attachment.Value);

        var email = new MimeMessage
        {
            Subject = emailMessage.Subject,
            Body = builder.ToMessageBody()
        };

        email.From.Add(new MailboxAddress(smtpSettings.SenderName, smtpSettings.SenderEmail));
        email.To.Add(new MailboxAddress(emailMessage.ToAddress.Split("@")[0], emailMessage.ToAddress));

        return email;
    }
}
