using N_Tier.Application.Common.Email;
using N_Tier.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Api.IntergrationTests.Helpers.Services
{
    public class EmailTestService : IEmailService
    {
        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            await Task.Delay(100);
        }
    }
}
