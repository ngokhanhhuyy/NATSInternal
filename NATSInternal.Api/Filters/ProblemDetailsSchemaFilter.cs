using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NATSInternal.Api.Filters;

public class ProblemDetailsSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(ProblemDetails))
        {
            schema.Properties["errors"] = new()
            {
                Type = "object",
                AdditionalProperties = new() { Type = "string" },
                Description = "A dictionary containing detailed validation or operation errors"
            };
        }
    }
}