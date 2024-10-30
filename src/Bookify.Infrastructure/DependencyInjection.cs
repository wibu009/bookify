using Asp.Versioning;
using Bookify.Application.Abstractions.Email;
using Bookify.Application.Abstractions.Time;
using Bookify.Infrastructure.Authentication;
using Bookify.Infrastructure.Authorization;
using Bookify.Infrastructure.Caching;
using Bookify.Infrastructure.Email;
using Bookify.Infrastructure.Jobs;
using Bookify.Infrastructure.Outbox;
using Bookify.Infrastructure.Persistence;
using Bookify.Infrastructure.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Bookify.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddTransient<IDateTimeProvider, DateTimeProvider>()
            .AddTransient<IEmailService, EmailService>()
            .AddPersistence(configuration)
            .AddAuthentication(configuration)
            .AddAuthorization(configuration)
            .AddCaching(configuration)
            .AddHealthChecks(configuration)
            .AddApiVersioning()
            .AddOutbox(configuration)
            .AddJobs();

        return services;
    }
    
    private static IServiceCollection AddApiVersioning(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
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
        
        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration)), tags: ["database"], name: "postgres")
            .AddRedis(configuration.GetConnectionString("Cache") ?? throw new ArgumentNullException(nameof(configuration)), tags: ["redis"], name: "redis")
            .AddUrlGroup(new Uri(configuration.GetSection("Keycloak:BaseUrl").Value ?? throw new ArgumentNullException(nameof(configuration))), name: "Keycloak", tags: ["keycloak"], httpMethod: HttpMethod.Get);
        
        return services;
    }
}