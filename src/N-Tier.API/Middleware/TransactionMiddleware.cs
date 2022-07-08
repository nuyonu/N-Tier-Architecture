using N_Tier.DataAccess.Persistence;

namespace N_Tier.API.Middleware;

public class TransactionMiddleware
{
    private readonly RequestDelegate _next;

    public TransactionMiddleware(RequestDelegate next)
    {
        _next = next;
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
