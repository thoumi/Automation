using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MimeKit;
using AutomationSystem.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AutomationSystem.Core.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _configuration;

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<List<EmailMessage>> GetUnreadEmailsAsync()
    {
        var messages = new List<EmailMessage>();

        try
        {
            using var client = new ImapClient();
            
            var host = _configuration["Email:ImapHost"] ?? "imap.outlook.com";
            var port = int.Parse(_configuration["Email:ImapPort"] ?? "993");
            var username = _configuration["Email:Username"] ?? "";
            var password = _configuration["Email:Password"] ?? "";

            await client.ConnectAsync(host, port, true);
            await client.AuthenticateAsync(username, password);

            var inbox = client.Inbox;
            await inbox.OpenAsync(MailKit.FolderAccess.ReadOnly);

            var uids = await inbox.SearchAsync(SearchQuery.NotSeen);

            foreach (var uid in uids.Take(50)) // Limiter à 50 messages
            {
                var message = await inbox.GetMessageAsync(uid);
                
                messages.Add(new EmailMessage
                {
                    Id = uid.ToString(),
                    From = message.From.ToString(),
                    Subject = message.Subject,
                    Body = message.TextBody ?? message.HtmlBody ?? "",
                    ReceivedDate = message.Date.DateTime,
                    HasAttachments = message.Attachments.Any()
                });
            }

            await client.DisconnectAsync(true);

            _logger.LogInformation($"Retrieved {messages.Count} unread emails");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unread emails");
            throw;
        }

        return messages;
    }

    public async Task<List<EmailAttachment>> GetEmailAttachmentsAsync(string emailId)
    {
        var attachments = new List<EmailAttachment>();

        try
        {
            using var client = new ImapClient();
            
            var host = _configuration["Email:ImapHost"] ?? "imap.outlook.com";
            var port = int.Parse(_configuration["Email:ImapPort"] ?? "993");
            var username = _configuration["Email:Username"] ?? "";
            var password = _configuration["Email:Password"] ?? "";

            await client.ConnectAsync(host, port, true);
            await client.AuthenticateAsync(username, password);

            var inbox = client.Inbox;
            await inbox.OpenAsync(MailKit.FolderAccess.ReadOnly);

            var uid = new MailKit.UniqueId(uint.Parse(emailId));
            var message = await inbox.GetMessageAsync(uid);

            foreach (var attachment in message.Attachments)
            {
                if (attachment is MimePart mimePart)
                {
                    using var memory = new MemoryStream();
                    await mimePart.Content.DecodeToAsync(memory);

                    attachments.Add(new EmailAttachment
                    {
                        FileName = mimePart.FileName ?? "attachment",
                        ContentType = mimePart.ContentType.MimeType,
                        Content = memory.ToArray()
                    });
                }
            }

            await client.DisconnectAsync(true);

            _logger.LogInformation($"Retrieved {attachments.Count} attachments from email {emailId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving attachments for email {emailId}");
            throw;
        }

        return attachments;
    }

    public async Task SendEmailAsync(string to, string subject, string body, List<string>? attachments = null)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                _configuration["Email:FromName"] ?? "Automation System",
                _configuration["Email:Username"] ?? ""
            ));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    builder.Attachments.Add(attachment);
                }
            }

            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            
            var smtpHost = _configuration["Email:SmtpHost"] ?? "smtp.outlook.com";
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var username = _configuration["Email:Username"] ?? "";
            var password = _configuration["Email:Password"] ?? "";

            await client.ConnectAsync(smtpHost, smtpPort, false);
            await client.AuthenticateAsync(username, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation($"Email sent successfully to {to}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending email to {to}");
            throw;
        }
    }

    public async Task MarkAsReadAsync(string emailId)
    {
        try
        {
            using var client = new ImapClient();
            
            var host = _configuration["Email:ImapHost"] ?? "imap.outlook.com";
            var port = int.Parse(_configuration["Email:ImapPort"] ?? "993");
            var username = _configuration["Email:Username"] ?? "";
            var password = _configuration["Email:Password"] ?? "";

            await client.ConnectAsync(host, port, true);
            await client.AuthenticateAsync(username, password);

            var inbox = client.Inbox;
            await inbox.OpenAsync(MailKit.FolderAccess.ReadWrite);

            var uid = new MailKit.UniqueId(uint.Parse(emailId));
            await inbox.StoreAsync(uid, new MailKit.StoreFlagsRequest(MailKit.StoreAction.Add, MailKit.MessageFlags.Seen));


            await client.DisconnectAsync(true);

            _logger.LogInformation($"Marked email {emailId} as read");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error marking email {emailId} as read");
            throw;
        }
    }
}

