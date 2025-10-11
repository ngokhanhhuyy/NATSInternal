using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NATSInternal.Api.Filters;

public class RemoveSchemaForNotFoundOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var response in operation.Responses)
        {
            if (response.Key == "404")
            {
                response.Value.Content.Clear();

                response.Value.Description = "Resource not found (no response body)";
            }

            if (response.Key == "204")
            {
                response.Value.Content.Clear();
                response.Value.Description = "No content";
            }
        }
    }
}