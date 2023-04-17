namespace MyApi.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using MyApi.Models;
using Microsoft.OpenApi.Any;

public class DefaultResponseOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Responses.Add("500", new OpenApiResponse { Description = "Internal server error" });
    }
}

public class BookSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(Book))
        {
            schema.Example = new OpenApiObject
            {
                ["Id"] = new OpenApiInteger(1),
                ["Title"] = new OpenApiString("The Catcher in the Rye"),
                ["ISBN"] = new OpenApiString("1234567890123"),
                ["AuthorId"] = new OpenApiInteger(1)
            };
        }
    }
}