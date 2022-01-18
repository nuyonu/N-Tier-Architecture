using System.Threading.Tasks;
using N_Tier.Application.Common.Email;
using N_Tier.Application.Services;

namespace N_Tier.Api.IntegrationTests.Helpers.Services;

public class EmailTestService : IEmailService
{
    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        await Task.Delay(100);
    }
}
