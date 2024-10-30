using Bookify.Api.Middleware;
using Bookify.Infrastructure.Persistence;

namespace Bookify.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task UpdateDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var databaseUpdater = scope.ServiceProvider.GetRequiredService<DatabaseUpdater>();
        await databaseUpdater.UpdateDatabaseAsync();
    }
    
    public static async Task SeedDataAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var databaseSeeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        await databaseSeeder.SeedDataAsync();
    }
    
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
    
    public static void UseRequestContextLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();
    }
}