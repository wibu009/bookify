using Bookify.Application.Abstractions.Persistent;
using Bookify.Infrastructure.Authentication;
using Bookify.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.Keycloak;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace Bookify.Application.IntegrationTests.Infrastructure;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("bookify_test_db")
        .WithUsername("postgres")
        .WithPassword("admin")
        .Build();
    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:latest")
        .Build();
    private readonly KeycloakContainer _keycloakContainer = new KeycloakBuilder()
        .WithResourceMapping(
            new FileInfo(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", ".files", "keycloak", "bookify-realm-export.json")),
            new FileInfo("/opt/keycloak/data/import/bookify-realm-export.json"))
        .WithCommand("--import-realm")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseNpgsql(_dbContainer.GetConnectionString())
                    .UseSnakeCaseNamingConvention();
            });

            services.RemoveAll(typeof(ISqlConnectionFactory));
            services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(_dbContainer.GetConnectionString()));

            services.Configure<RedisCacheOptions>(options =>
            {
                options.Configuration = _redisContainer.GetConnectionString();
            });

            var keycloakAddress = _keycloakContainer.GetBaseAddress();
            services.Configure<KeycloakOptions>(options =>
            {
                options.AdminUrl = $"{keycloakAddress}/admin/realms/bookify/";
                options.TokenUrl = $"{keycloakAddress}/realms/bookify/protocol/openid-connect/token";
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _redisContainer.StartAsync();
        await _keycloakContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _keycloakContainer.StopAsync();
        await _redisContainer.StopAsync();
        await _dbContainer.StopAsync();
    }
}