using N_Tier.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Api.IntergrationTests.Helpers.Services
{
    public class TemplateTestService : ITemplateService
    {
        public async Task<string> GetTemplateAsync(string templateName)
        {
            await Task.Delay(100);
            return "{UserId} + {Token}";
        }

        public string ReplaceInTemplate(string input, IDictionary<string, string> replaceWords)
        {
            var response = string.Empty;

            foreach (var temp in replaceWords)
                response = input.Replace(temp.Key, temp.Value);

            return response;
        }
    }
}
