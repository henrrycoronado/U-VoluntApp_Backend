namespace U_VoluntApp_Backend.Src.Presentation.Middleware;

using System.Diagnostics;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        context.Response.OnStarting(() =>
        {
            stopwatch.Stop();
            context.Response.Headers.Append("X-Response-Time-Ms", stopwatch.ElapsedMilliseconds.ToString());
            return Task.CompletedTask;
        });

        try
        {
            await _next(context);
        }
        finally
        {
            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
            }

            var statusCode = context.Response.StatusCode;
            var method = context.Request.Method;
            var path = context.Request.Path;
            var elapsed = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation(
                "[HTTP {Method}] {Path} -> {StatusCode} ({Elapsed}ms)",
                method,
                path,
                statusCode,
                elapsed);
        }
    }
}
