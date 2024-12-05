namespace Bookify.Application.Abstractions.Email;

public interface IEmailService
{
    public Task SendAsync(
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
        CancellationToken ct = default);
}