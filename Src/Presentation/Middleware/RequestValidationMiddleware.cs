namespace U_VoluntApp_Backend.Src.Presentation.Middleware;

using System.Net;
using System.Text;
using System.Text.Json;

public static class RequestValidationMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestValidation(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestValidationMiddleware>();
    }
}

public class RequestValidationMiddleware
{
    private const long MaxBodySizeBytes = 10 * 1024 * 1024;
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestValidationMiddleware> _logger;

    public RequestValidationMiddleware(RequestDelegate next, ILogger<RequestValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;

        if (request.Method is "POST" or "PUT" or "PATCH")
        {
            if (request.ContentLength.HasValue && request.ContentLength > MaxBodySizeBytes)
            {
                _logger.LogWarning("Body too large: {Size} bytes for {Method} {Path}", request.ContentLength, request.Method, request.Path);

                await WriteValidationErrorResponse(
                    context,
                    $"El body no puede exceder {MaxBodySizeBytes / (1024 * 1024)} MB",
                    (HttpStatusCode)413);
                return;
            }

            if (request.ContentLength > 0)
            {
                var bodyReader = new StreamReader(request.Body);
                var body = await bodyReader.ReadToEndAsync();

                if (!string.IsNullOrWhiteSpace(body))
                {
                    try
                    {
                        JsonSerializer.Deserialize<JsonElement>(body);
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogWarning("Invalid JSON format: {Error} for {Method} {Path}", ex.Message, request.Method, request.Path);
                        await WriteValidationErrorResponse(
                            context,
                            "JSON inválido en el body",
                            HttpStatusCode.BadRequest);
                        return;
                    }
                }

                request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
            }
        }

        if (request.Method is "GET" or "DELETE")
        {
            if (request.ContentLength > 0)
            {
                _logger.LogWarning("{Method} request should not have body for {Path}", request.Method, request.Path);
                await WriteValidationErrorResponse(
                    context,
                    $"{request.Method} requests no deben tener body",
                    HttpStatusCode.BadRequest);
                return;
            }
        }

        await _next(context);
    }

    private static async Task WriteValidationErrorResponse(HttpContext context, string message, HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            error = message,
            code = (int)statusCode,
        };
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}
