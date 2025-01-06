using Asp.Versioning;
using Bookify.Api.OpenApi;

namespace Bookify.Api.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddHttpContextAccessor();
        
        services.AddApiVersioning(options => 
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
        
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        
        return services;
    }
}