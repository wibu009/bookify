using Bookify.Application.Abstractions.Email;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Logging;

namespace Bookify.Infrastructure.Email;

internal sealed class EmailService(
    IFluentEmail email,
    ILogger<EmailService> logger) 
    : IEmailService
{
    public async Task SendAsync(
        IEnumerable<string> to,
        string subject,
        string body,
        string? from = null,
        IEnumerable<string>? cc = null,
        IEnumerable<string>? bcc = null,
        string? replyTo = null,
        string? replyToName = null,
        string? displayName = null,
        Dictionary<string, string>? headers = null,
        Dictionary<string, byte[]>? attachmentData = null,
        CancellationToken ct = default)
    {
        var mail = email
            .To(to.Select(address => new Address(address)).ToList())
            .Subject(subject)
            .Body(body, true);
        
        // Add From
        if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(displayName))
        {
            mail.SetFrom(from, displayName);
        }

        // Add ReplyTo
        if (!string.IsNullOrEmpty(replyTo))
        {
            mail.ReplyTo(replyTo, replyToName ?? string.Empty);
        }

        // Add CC
        if (cc != null)
        {
            mail.CC(cc.Select(address => new Address(address)).ToList());
        }

        // Add BCC
        if (bcc != null)
        {
            mail.BCC(bcc.Select(address => new Address(address)).ToList());
        }

        // Add Headers
        if (headers != null)
        {
            foreach (var header in headers)
            {
                mail.Header(header.Key, header.Value);
            }
        }

        // Add Attachments
        if (attachmentData != null)
        {
            foreach (var attachment in attachmentData)
            {
                mail.Attach(new Attachment
                {
                    Filename = attachment.Key,
                    Data = new MemoryStream(attachment.Value)
                });
            }
        }

        try
        {
            // Send the email
            var response = await mail.SendAsync(ct);
            if (!response.Successful)
            {
                foreach (var error in response.ErrorMessages)
                {
                    logger.LogError("Failed to send email: {ErrorMessage}", error);
                }

                throw new Exception($"Email sending failed. Errors: {string.Join(", ", response.ErrorMessages)}");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while sending email: {Message}", ex.Message);
            throw;
        }
    }

}