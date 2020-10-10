using N_Tier.Application.Common.Email;
using System.Threading.Tasks;

namespace N_Tier.Application.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage emailMessage);
    }
}
