using AutomationSystem.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace AutomationSystem.Core.Services;

public class WhatsAppService : IWhatsAppService
{
    private readonly ILogger<WhatsAppService> _logger;
    private readonly IConfiguration _configuration;

    public WhatsAppService(ILogger<WhatsAppService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        var accountSid = _configuration["Twilio:AccountSid"] ?? "";
        var authToken = _configuration["Twilio:AuthToken"] ?? "";

        TwilioClient.Init(accountSid, authToken);
    }

    public async Task<bool> SendTextMessageAsync(string phoneNumber, string message)
    {
        try
        {
            var from = _configuration["Twilio:WhatsAppNumber"] ?? "";
            
            var messageResource = await MessageResource.CreateAsync(
                to: new PhoneNumber($"whatsapp:{phoneNumber}"),
                from: new PhoneNumber($"whatsapp:{from}"),
                body: message
            );

            _logger.LogInformation($"WhatsApp message sent to {phoneNumber}, SID: {messageResource.Sid}");

            return messageResource.Status != MessageResource.StatusEnum.Failed;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending WhatsApp message to {phoneNumber}");
            return false;
        }
    }

    public async Task<bool> SendImageMessageAsync(string phoneNumber, byte[] imageData, string caption = "")
    {
        try
        {
            var from = _configuration["Twilio:WhatsAppNumber"] ?? "";
            
            // Note: Pour envoyer une image, il faut d'abord l'héberger sur une URL accessible
            // Cette implémentation suppose que vous avez un service de stockage
            _logger.LogWarning("Image upload to storage not implemented. Using text fallback.");
            
            return await SendTextMessageAsync(phoneNumber, caption);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending WhatsApp image to {phoneNumber}");
            return false;
        }
    }

    public async Task<bool> SendDocumentAsync(string phoneNumber, byte[] documentData, string fileName)
    {
        try
        {
            var from = _configuration["Twilio:WhatsAppNumber"] ?? "";
            
            // Note: Similaire aux images, les documents nécessitent une URL accessible
            _logger.LogWarning("Document upload to storage not implemented. Using text fallback.");
            
            return await SendTextMessageAsync(phoneNumber, $"Document: {fileName}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending WhatsApp document to {phoneNumber}");
            return false;
        }
    }

    public async Task<List<WhatsAppRecipient>> GetRecipientsAsync()
    {
        // Cette méthode devrait récupérer les destinataires depuis la base de données
        await Task.CompletedTask;
        
        return new List<WhatsAppRecipient>();
    }
}

