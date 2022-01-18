using N_Tier.Application.Common.Email;

namespace N_Tier.Application.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);
}
