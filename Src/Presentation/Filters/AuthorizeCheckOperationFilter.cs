using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace U_VoluntApp_Core.Src.Presentation.Filters;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAllowAnonymous = context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any() == true ||
                                context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();

        if (hasAllowAnonymous)
        {
            operation.Description += "\n\n**🔓 Acceso Público (Sin autenticación requerida)**";
            return;
        }

        var authorizeAttributes = (context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>() ?? Enumerable.Empty<AuthorizeAttribute>())
            .Union(context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>());

        if (authorizeAttributes.Any())
        {
            var roles = authorizeAttributes
                .Where(a => !string.IsNullOrEmpty(a.Roles))
                .SelectMany(a => a.Roles!.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                .Distinct()
                .ToList();

            if (roles.Any())
            {
                operation.Description += $"\n\n**🔒 Roles Requeridos:** `{string.Join("`, `", roles)}`";
            }
            else
            {
                operation.Description += "\n\n**🔒 Requiere Autenticación (Cualquier rol)**";
            }
        }
    }
}
