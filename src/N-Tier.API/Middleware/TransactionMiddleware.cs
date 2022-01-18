using N_Tier.DataAccess.Persistence;

namespace N_Tier.API.Middleware;

public class TransactionMiddleware
{
    private readonly ILogger<TransactionMiddleware> _logger;

    private readonly RequestDelegate _next;

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
