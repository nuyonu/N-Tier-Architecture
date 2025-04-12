using N_Tier.DataAccess.Persistence;

namespace N_Tier.API.Middleware;

public class TransactionMiddleware(RequestDelegate next, ILogger<TransactionMiddleware> logger)
{
    private readonly ILogger<TransactionMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context, DatabaseContext databaseContext)
    {
        await using var transaction = await databaseContext.Database.BeginTransactionAsync();

        try
        {
            await next(context);

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }
}
