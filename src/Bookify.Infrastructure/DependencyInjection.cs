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
            .AddPersistence(configuration)
            .AddCaching(configuration)
            .AddAuthentication(configuration)
            .AddAuthorization(configuration)
            .AddHealthChecks(configuration)
            .AddEmail(configuration)
            .AddOutbox(configuration)
            .AddJobs();

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