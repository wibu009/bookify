using System.Reflection;
using Asp.Versioning;
using Bookify.Api.Extensions;
using Bookify.Application;
using Bookify.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation();

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .ReportApiVersions()
    .Build();
var versionedGroups = app.MapGroup("/api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);
app.MapEndpoints(versionedGroups);

await app.UpdateDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();
    await app.SeedDataAsync();
}

app.UseHttpsRedirection();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}); 

app.UseRequestContextLogging();
app.UseSerilogRequestLogging();

app.UseCustomExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

public partial class Program;
