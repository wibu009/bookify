using Scalar.AspNetCore;

namespace Bookify.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerUi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var descriptions = app.DescribeApiVersions();

            foreach (var description in descriptions)
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                var name = description.GroupName.ToUpperInvariant();

                options.SwaggerEndpoint(url, name);
            }
        });

        return app;
    }

    public static IApplicationBuilder UseScalarUi(this WebApplication app)
    {
        app.UseSwagger(options =>
        {
            options.RouteTemplate = "/openapi/{documentName}.json";
        });
        app.MapScalarApiReference(options =>
        {
            options
                .WithLayout(ScalarLayout.Modern)
                .WithTheme(ScalarTheme.DeepSpace);
        });
        
        return app;
    }
}