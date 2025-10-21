using AutomationSystem.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace AutomationSystem.Core.Services;

public class ChimeService : IChimeService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ChimeService> _logger;
    private readonly IConfiguration _configuration;

    public ChimeService(HttpClient httpClient, ILogger<ChimeService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<bool> SendMessageAsync(string message)
    {
        try
        {
            var webhookUrl = _configuration["Chime:WebhookUrl"] ?? "";

            var payload = new
            {
                Content = message
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(webhookUrl, content);
            
            response.EnsureSuccessStatusCode();

            _logger.LogInformation("Chime message sent successfully");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending Chime message");
            return false;
        }
    }

    public async Task<bool> SendMessageWithImageAsync(string message, byte[] imageData)
    {
        try
        {
            var webhookUrl = _configuration["Chime:WebhookUrl"] ?? "";

            // Amazon Chime webhooks supportent le markdown
            // Pour les images, il faut les héberger et utiliser la syntaxe markdown
            var payload = new
            {
                Content = $"{message}\n\n(Image attachée - {imageData.Length} bytes)"
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(webhookUrl, content);
            
            response.EnsureSuccessStatusCode();

            _logger.LogInformation("Chime message with image sent successfully");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending Chime message with image");
            return false;
        }
    }

    public async Task<bool> SendAlertAsync(string title, string message, AlertLevel level)
    {
        try
        {
            var emoji = level switch
            {
                AlertLevel.Info => "ℹ️",
                AlertLevel.Warning => "⚠️",
                AlertLevel.Error => "❌",
                AlertLevel.Critical => "🚨",
                _ => "📢"
            };

            var formattedMessage = $"{emoji} **{title}**\n\n{message}";

            return await SendMessageAsync(formattedMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending Chime alert");
            return false;
        }
    }
}

