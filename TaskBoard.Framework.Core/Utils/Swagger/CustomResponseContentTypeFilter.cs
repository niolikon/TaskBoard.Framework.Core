using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TaskBoard.Framework.Core.Dtos;

namespace TaskBoard.Framework.Core.Utils.Swagger;

public class CustomResponseContentTypeFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var response in operation.Responses)
        {
            if (response.Key.StartsWith("4"))
            {
                if (!response.Value.Content.ContainsKey("application/json"))
                {
                    response.Value.Content.Add("application/json", new OpenApiMediaType());
                }

                operation.Responses[response.Key].Content["application/json"].Schema =
                    context.SchemaGenerator.GenerateSchema(typeof(ErrorDto), context.SchemaRepository);
            }
            else if (response.Key.StartsWith("2") && (response.Key != "204"))
            {
                if (!response.Value.Content.ContainsKey("application/json"))
                {
                    response.Value.Content.Add("application/json", new OpenApiMediaType());
                }
            }
            else
            {
                response.Value.Content.Remove("application/json");
            }
        }
    }
}
