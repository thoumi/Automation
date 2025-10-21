using AutomationSystem.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace AutomationSystem.Core.Jobs;

/// <summary>
/// Tâche 3: Traitement des DNR Units (déclenchée par email)
/// </summary>
public class DNRUnitsJob
{
    private readonly IEmailService _emailService;
    private readonly IExcelService _excelService;
    private readonly IChimeService _chimeService;
    private readonly ILogger<DNRUnitsJob> _logger;

    public DNRUnitsJob(
        IEmailService emailService,
        IExcelService excelService,
        IChimeService chimeService,
        ILogger<DNRUnitsJob> logger)
    {
        _emailService = emailService;
        _excelService = excelService;
        _chimeService = chimeService;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        try
        {
            _logger.LogInformation("Starting DNR Units job at {Time}", DateTime.Now);

            // 1. Récupérer les emails non lus
            var emails = await _emailService.GetUnreadEmailsAsync();
            
            _logger.LogInformation("Found {Count} unread emails", emails.Count);

            var processedCount = 0;

            foreach (var email in emails)
            {
                // 2. Filtrer les emails concernant DNR Units
                if (email.Subject.Contains("DNR", StringComparison.OrdinalIgnoreCase) ||
                    email.Subject.Contains("Units", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogInformation("Processing email: {Subject}", email.Subject);

                    // 3. Récupérer les pièces jointes
                    if (email.HasAttachments)
                    {
                        var attachments = await _emailService.GetEmailAttachmentsAsync(email.Id);
                        
                        foreach (var attachment in attachments)
                        {
                            // 4. Traiter les fichiers Excel
                            if (attachment.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) ||
                                attachment.FileName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
                            {
                                // Sauvegarder temporairement le fichier
                                var tempPath = Path.Combine(Path.GetTempPath(), attachment.FileName);
                                await File.WriteAllBytesAsync(tempPath, attachment.Content);

                                try
                                {
                                    // Lire les données
                                    var data = await _excelService.ReadExcelFileAsync(tempPath);
                                    
                                    _logger.LogInformation("Processed {Count} rows from {FileName}", 
                                        data.Count, attachment.FileName);

                                    // 5. Envoyer un rapport sur Chime
                                    var message = $"📧 DNR Units reçu\n\n";
                                    message += $"📨 De: {email.From}\n";
                                    message += $"📎 Fichier: {attachment.FileName}\n";
                                    message += $"📊 Lignes: {data.Count}\n";
                                    message += $"🕐 Reçu: {email.ReceivedDate:dd.MM.yyyy HH:mm}";

                                    await _chimeService.SendMessageAsync(message);

                                    processedCount++;
                                }
                                finally
                                {
                                    // Nettoyer le fichier temporaire
                                    if (File.Exists(tempPath))
                                    {
                                        File.Delete(tempPath);
                                    }
                                }
                            }
                        }
                    }

                    // 6. Marquer l'email comme lu
                    await _emailService.MarkAsReadAsync(email.Id);
                }
            }

            _logger.LogInformation("DNR Units job completed. Processed {Count} emails", processedCount);

            if (processedCount > 0)
            {
                await _chimeService.SendAlertAsync(
                    "DNR Units traités",
                    $"{processedCount} email(s) DNR Units traité(s) avec succès",
                    AlertLevel.Info
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing DNR Units job");
            
            await _chimeService.SendAlertAsync(
                "Erreur DNR Units",
                $"Une erreur s'est produite: {ex.Message}",
                AlertLevel.Error
            );
            
            throw;
        }
    }
}

