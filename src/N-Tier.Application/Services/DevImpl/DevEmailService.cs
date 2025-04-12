using Microsoft.Extensions.Logging;
using N_Tier.Application.Common.Email;

namespace N_Tier.Application.Services.DevImpl;

public class DevEmailService(ILogger<DevEmailService> logger) : IEmailService
{
    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        await Task.Delay(100);

        logger.LogInformation($"Email was sent to: [{emailMessage.ToAddress}]. Body: {emailMessage.Body}");
    }
}
