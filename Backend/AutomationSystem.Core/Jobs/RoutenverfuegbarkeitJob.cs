using AutomationSystem.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace AutomationSystem.Core.Jobs;

/// <summary>
/// Tâche 1: Envoi de Routenverfügbarkeit à 08h25
/// </summary>
public class RoutenverfuegbarkeitJob
{
    private readonly IExcelService _excelService;
    private readonly IChimeService _chimeService;
    private readonly IWhatsAppService _whatsAppService;
    private readonly ILogger<RoutenverfuegbarkeitJob> _logger;

    public RoutenverfuegbarkeitJob(
        IExcelService excelService,
        IChimeService chimeService,
        IWhatsAppService whatsAppService,
        ILogger<RoutenverfuegbarkeitJob> logger)
    {
        _excelService = excelService;
        _chimeService = chimeService;
        _whatsAppService = whatsAppService;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        try
        {
            _logger.LogInformation("Starting Routenverfügbarkeit job at {Time}", DateTime.Now);

            // 1. Lire le fichier Excel
            var filePath = "uploads/routenverfuegbarkeit.xlsx"; // Chemin configurable
            var data = await _excelService.ReadExcelFileAsync(filePath);

            // 2. Générer une image du tableau
            var imageData = await _excelService.ConvertExcelToImageAsync(filePath);

            // 3. Préparer le message
            var message = $"📋 Routenverfügbarkeit - {DateTime.Now:dd.MM.yyyy}\n\n";
            message += $"Total des entrées: {data.Count}";

            // 4. Envoyer sur Chime
            if (imageData.Length > 0)
            {
                await _chimeService.SendMessageWithImageAsync(message, imageData);
            }
            else
            {
                await _chimeService.SendMessageAsync(message);
            }

            // 5. Envoyer sur WhatsApp aux destinataires configurés
            var recipients = await _whatsAppService.GetRecipientsAsync();
            foreach (var recipient in recipients.Where(r => r.IsActive))
            {
                if (imageData.Length > 0)
                {
                    await _whatsAppService.SendImageMessageAsync(recipient.PhoneNumber, imageData, message);
                }
                else
                {
                    await _whatsAppService.SendTextMessageAsync(recipient.PhoneNumber, message);
                }
            }

            _logger.LogInformation("Routenverfügbarkeit job completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing Routenverfügbarkeit job");
            
            // Envoyer une alerte en cas d'erreur
            await _chimeService.SendAlertAsync(
                "Erreur Routenverfügbarkeit",
                $"Une erreur s'est produite: {ex.Message}",
                AlertLevel.Error
            );
            
            throw;
        }
    }
}

