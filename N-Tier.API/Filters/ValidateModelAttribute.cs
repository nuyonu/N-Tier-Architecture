using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace N_Tier.API.Filters
{
    public class ValidateModelAttribute : Attribute, IAsyncResultFilter
    {
        private readonly ILogger<ValidateModelAttribute> _logger;

        public ValidateModelAttribute(ILogger<ValidateModelAttribute> logger)
        {
            _logger = logger;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                _logger.LogWarning("Model is invalid");
                // TODO same response
                context.Result = new BadRequestObjectResult(context.ModelState);
            }

            await next();
        }
    }
}
