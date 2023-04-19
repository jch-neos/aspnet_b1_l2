namespace MyApi.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using MyApi.Models;
using Microsoft.OpenApi.Any;
using Microsoft.AspNetCore.Authorization;

/// <summary> add default response for all operations </summary>
public class DefaultResponseOperationFilter : IOperationFilter
{
    /// <summary> apply the filter </summary>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Responses.Add("500", new OpenApiResponse { Description = "Internal server error" });
    }
}

/// <summary> add example data for books </summary>
public class BookSchemaFilter : ISchemaFilter
{
    /// <summary> apply the filter </summary>
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

internal class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check for authorize attribute
            var hasAuthorize = context.ApiDescription.CustomAttributes().OfType<AuthorizeAttribute>().Any();
            if (hasAuthorize)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {{
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        }, new [] { "DataRead" }
                    }}
                };
        }
        }
    }