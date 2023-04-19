using System.Text.Json;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class ConfigureSwaggerOptions
    : IConfigureOptions<Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(
        IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Configure each API discovered for Swagger Documentation
    /// </summary>
    /// <param name="options"></param>
    public void Configure(SwaggerGenOptions options)
    {
        // add swagger document for every API version discovered
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                CreateVersionInfo(description));
        }
    }

    /// <summary>
    /// Configure Swagger Options. Inherited from the Interface
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    public void Configure(string name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    /// <summary>
    /// Create information about the version of the API
    /// </summary>
    /// <param name="description"></param>
    /// <returns>Information about the API</returns>
    private OpenApiInfo CreateVersionInfo(
            ApiVersionDescription desc)
    {
        var info = new OpenApiInfo()
        {
            Title = "Book Web API",
            Version = desc.ApiVersion.ToString()
        };

        if (desc.IsDeprecated)
        {
            info.Description += " This API version has been deprecated. Please use one of the new APIs available from the explorer.";
        }

        return info;
    }
}

public class SwaggerDefaultValues : IOperationFilter
{
  public void Apply( OpenApiOperation operation, OperationFilterContext context )
  {
    var apiDescription = context.ApiDescription;

    foreach ( var responseType in context.ApiDescription.SupportedResponseTypes )
    {
        var responseKey = responseType.IsDefaultResponse
                          ? "default"
                          : responseType.StatusCode.ToString();
        var response = operation.Responses[responseKey];

        foreach ( var contentType in response.Content.Keys )
        {
            if ( !responseType.ApiResponseFormats.Any( x => x.MediaType == contentType ) )
            {
                response.Content.Remove( contentType );
            }
        }
    }

    if ( operation.Parameters == null )
    {
        return;
    }

    foreach ( var parameter in operation.Parameters )
    {
        var description = apiDescription.ParameterDescriptions
                                        .First( p => p.Name == parameter.Name );

        parameter.Description ??= description.ModelMetadata?.Description;

        if ( parameter.Schema.Default == null && description.DefaultValue != null )
        {
            var json = JsonSerializer.Serialize(
                description.DefaultValue,
                description.ModelMetadata.ModelType );
            parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson( json );
        }

        parameter.Required |= description.IsRequired;
    }
  }
}