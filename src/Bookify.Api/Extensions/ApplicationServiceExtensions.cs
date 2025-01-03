using Bookify.Api.OpenApi;

namespace Bookify.Api.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddHttpContextAccessor();
        
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        
        return services;
    }
}