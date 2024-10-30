using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Bookify.Api.OpenApi;

public sealed class ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    : IConfigureNamedOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
        }
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }
    
    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Bookify API - Version " + description.ApiVersion,
            Version = description.ApiVersion.ToString(),
            Description = "Bookify Api",
            Contact = new OpenApiContact
            {
                Name = "Bookify",
                Email = string.Empty
            }
        };
        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated";
        }
        
        return info;
    }
}   