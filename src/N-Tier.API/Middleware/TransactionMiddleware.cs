using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using N_Tier.DataAccess.Persistence;
using System.Threading.Tasks;

namespace N_Tier.API.Middleware
{
    public class TransactionMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<TransactionMiddleware> _logger;

        public TransactionMiddleware(RequestDelegate next, ILogger<TransactionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, DatabaseContext databaseContext)
        {
            await using var transaction = await databaseContext.Database.BeginTransactionAsync();

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
