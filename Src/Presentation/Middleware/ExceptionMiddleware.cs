namespace U_VoluntApp_Core.Src.Presentation.Middleware;

using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Recurso no encontrado: {Message}", ex.Message);
            await WriteErrorResponse(context, HttpStatusCode.NotFound, "Recurso no encontrado", ex.Message);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Argumento inválido: {Message}", ex.Message);
            await WriteErrorResponse(context, HttpStatusCode.BadRequest, "Argumento inválido", ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Operación inválida: {Message}", ex.Message);
            await WriteErrorResponse(context, HttpStatusCode.BadRequest, "Operacion invalida", ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "No autorizado: {Message}", ex.Message);
            await WriteErrorResponse(context, HttpStatusCode.Forbidden, "No autorizado", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado: {Message}", ex.Message);
            await WriteErrorResponse(context, HttpStatusCode.InternalServerError, "Error interno del servidor", "Ocurrio un error inesperado. Intenta nuevamente o contacta a soporte.");
        }
    }

    private static async Task WriteErrorResponse(HttpContext context, HttpStatusCode statusCode, string title, string detail)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };
        problemDetails.Extensions["traceId"] = context.TraceIdentifier;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, options));
    }
}
