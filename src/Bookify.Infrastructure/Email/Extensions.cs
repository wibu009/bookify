using Bookify.Application.Abstractions.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure.Email;

public static class Extensions
{
    public static IServiceCollection AddEmail(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("Email").Get<EmailOptions>()
            ?? throw new ArgumentNullException(nameof(configuration));
        
        services.AddFluentEmail(options.From, options.DisplayName)
            .AddSmtpSender(options.Host, options.Port, options.UserName, options.Password)
            .AddRazorRenderer();
        services.AddScoped<IEmailService, EmailService>();
        
        return services;
    }
}