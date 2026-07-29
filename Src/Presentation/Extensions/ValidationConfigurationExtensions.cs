namespace U_VoluntApp_Core.Src.Presentation.Extensions;

using Microsoft.AspNetCore.Mvc;

public static class ValidationConfigurationExtensions
{
    public static IMvcBuilder AddCustomModelValidation(this IMvcBuilder builder)
    {
        return builder.ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var message = "Error de validación: " + string.Join(" | ", errors);

                if (context.HttpContext.Request.Path.Value?.Contains("/auth/", StringComparison.OrdinalIgnoreCase) == true)
                {
                    message = "Credenciales o datos de acceso inválidos.";
                }

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Error de validación de modelo",
                    Detail = message,
                    Instance = context.HttpContext.Request.Path
                };
                problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

                return new BadRequestObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json" }
                };
            };
        });
    }
}
