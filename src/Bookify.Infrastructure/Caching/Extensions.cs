using Bookify.Application.Abstractions.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Bookify.Infrastructure.Caching;

public static class Extensions
{
    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Cache")
                               ?? throw new ArgumentNullException(nameof(configuration));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
            options.ConfigurationOptions = new ConfigurationOptions
            {
                EndPoints = { connectionString },
                ConnectTimeout = 5000,
                SyncTimeout = 5000,
                AbortOnConnectFail = false
            };
        });
        services.AddSingleton<ICacheService, DistributedCacheService>();
        
        return services;
    }
}