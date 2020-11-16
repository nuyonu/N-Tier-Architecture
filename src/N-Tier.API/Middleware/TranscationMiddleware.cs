using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using N_Tier.DataAccess.Persistence;
using System.Threading.Tasks;

namespace N_Tier.API.Middleware
{
    public class TranscationMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<TranscationMiddleware> _logger;
        private readonly DatabaseContext _databaseContext;

        public TranscationMiddleware(RequestDelegate next, ILogger<TranscationMiddleware> logger, DatabaseContext databaseContext)
        {
            _next = next;
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task Invoke(HttpContext context)
        {
            await using var transaction = await _databaseContext.Database.BeginTransactionAsync();

            try
            {
                await _next(context);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
            }
        }
    }
}
