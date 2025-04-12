using System.Diagnostics;

namespace N_Tier.API.Middleware;

public class PerformanceMiddleware(RequestDelegate next, ILogger<PerformanceMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        const int performanceTimeLog = 500;

        var sw = new Stopwatch();

        sw.Start();

        await next(context);

        sw.Stop();

        if (performanceTimeLog < sw.ElapsedMilliseconds)
            logger.LogWarning("Request {method} {path} it took about {elapsed} ms",
                context.Request?.Method,
                context.Request?.Path.Value,
                sw.ElapsedMilliseconds);
    }
}
