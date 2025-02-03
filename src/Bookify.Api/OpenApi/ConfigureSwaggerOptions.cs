using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Bookify.Api.OpenApi;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public sealed class ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    : IConfigureNamedOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        AddJwtBearerAuthentication(options);
        AddApiVersionDocumentation(options);
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private static void AddJwtBearerAuthentication(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer' followed by your token. Example: 'Bearer {token}'"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                },
                Array.Empty<string>()
            }
        });
    }

    private void AddApiVersionDocumentation(SwaggerGenOptions options)
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, GenerateApiVersionInfo(description));
        }
    }

    private static OpenApiInfo GenerateApiVersionInfo(ApiVersionDescription description)
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