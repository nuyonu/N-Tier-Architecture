using System.Threading.Tasks;

namespace N_Tier.Application.Services
{
    public interface ITemplateService
    {
        Task<string> GetTemplateAsync(string templateName);
    }
}
