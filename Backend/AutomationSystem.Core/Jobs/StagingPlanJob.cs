using AutomationSystem.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace AutomationSystem.Core.Jobs;

/// <summary>
/// Tâche 2: Traitement du Staging Plan à 09h15
/// </summary>
public class StagingPlanJob
{
    private readonly IExcelService _excelService;
    private readonly ICortexService _cortexService;
    private readonly IChimeService _chimeService;
    private readonly ILogger<StagingPlanJob> _logger;

    public StagingPlanJob(
        IExcelService excelService,
        ICortexService cortexService,
        IChimeService chimeService,
        ILogger<StagingPlanJob> logger)
    {
        _excelService = excelService;
        _cortexService = cortexService;
        _chimeService = chimeService;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        try
        {
            _logger.LogInformation("Starting Staging Plan job at {Time}", DateTime.Now);

            // 1. Lire le fichier Excel du Staging Plan
            var filePath = "uploads/staging_plan.xlsx";
            var data = await _excelService.ReadExcelFileAsync(filePath);

            _logger.LogInformation("Read {Count} rows from Staging Plan", data.Count);

            // 2. Récupérer les données de CORTEX pour comparaison
            var tours = await _cortexService.GetToursAsync(DateTime.Today);
            
            _logger.LogInformation("Retrieved {Count} tours from CORTEX", tours.Count);

            // 3. Analyser les différences et anomalies
            var anomalies = new List<string>();
            
            foreach (var row in data)
            {
                // Logique de comparaison personnalisée selon vos besoins
                // Exemple: vérifier si une tournée existe dans CORTEX
                if (row.ContainsKey("TourId"))
                {
                    var tourId = row["TourId"].ToString();
                    var cortexTour = tours.FirstOrDefault(t => t.TourId == tourId);
                    
                    if (cortexTour == null)
                    {
                        anomalies.Add($"Tournée {tourId} manquante dans CORTEX");
                    }
                }
            }

            // 4. Générer un rapport
            var message = $"📊 Staging Plan - {DateTime.Now:dd.MM.yyyy HH:mm}\n\n";
            message += $"✅ Lignes traitées: {data.Count}\n";
            message += $"🚚 Tournées CORTEX: {tours.Count}\n";
            
            if (anomalies.Any())
            {
                message += $"\n⚠️ Anomalies détectées ({anomalies.Count}):\n";
                message += string.Join("\n", anomalies.Take(10));
                
                if (anomalies.Count > 10)
                {
                    message += $"\n... et {anomalies.Count - 10} autres";
                }
            }
            else
            {
                message += "\n✅ Aucune anomalie détectée";
            }

            // 5. Envoyer le rapport sur Chime
            await _chimeService.SendMessageAsync(message);

            // 6. Générer et envoyer une image du rapport
            var imageData = await _excelService.ConvertExcelToImageAsync(filePath);
            if (imageData.Length > 0)
            {
                await _chimeService.SendMessageWithImageAsync("Staging Plan détaillé", imageData);
            }

            _logger.LogInformation("Staging Plan job completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing Staging Plan job");
            
            await _chimeService.SendAlertAsync(
                "Erreur Staging Plan",
                $"Une erreur s'est produite lors du traitement: {ex.Message}",
                AlertLevel.Error
            );
            
            throw;
        }
    }
}

