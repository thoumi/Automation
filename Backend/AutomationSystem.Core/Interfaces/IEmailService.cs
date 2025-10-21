using AutomationSystem.Core.Models;

namespace AutomationSystem.Core.Interfaces;

public interface IEmailService
{
    Task<List<EmailMessage>> GetUnreadEmailsAsync();
    Task<List<EmailAttachment>> GetEmailAttachmentsAsync(string emailId);
    Task SendEmailAsync(string to, string subject, string body, List<string>? attachments = null);
    Task MarkAsReadAsync(string emailId);
}

public class EmailMessage
{
    public string Id { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime ReceivedDate { get; set; }
    public bool HasAttachments { get; set; }
}

public class EmailAttachment
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public byte[] Content { get; set; } = Array.Empty<byte>();
}

