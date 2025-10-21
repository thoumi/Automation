using ClosedXML.Excel;
using AutomationSystem.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace AutomationSystem.Core.Services;

public class ExcelService : IExcelService
{
    private readonly ILogger<ExcelService> _logger;

    public ExcelService(ILogger<ExcelService> logger)
    {
        _logger = logger;
    }

    public async Task<List<Dictionary<string, object>>> ReadExcelFileAsync(string filePath)
    {
        return await Task.Run(() =>
        {
            var result = new List<Dictionary<string, object>>();

            try
            {
                using var workbook = new XLWorkbook(filePath);
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RangeUsed().RowsUsed().ToList();

                if (rows.Count == 0) return result;

                // Première ligne = en-têtes
                var headers = rows[0].Cells()
                    .Select(c => c.Value.ToString())
                    .ToList();

                // Lignes de données
                for (int i = 1; i < rows.Count; i++)
                {
                    var row = rows[i];
                    var rowData = new Dictionary<string, object>();

                    var cells = row.Cells().ToList();
                    for (int j = 0; j < headers.Count && j < cells.Count; j++)
                    {
                        rowData[headers[j]] = cells[j].Value;
                    }

                    result.Add(rowData);
                }

                _logger.LogInformation($"Successfully read {result.Count} rows from {filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error reading Excel file: {filePath}");
                throw;
            }

            return result;
        });
    }

    public async Task<List<T>> ReadExcelFileAsync<T>(string filePath) where T : class, new()
    {
        return await Task.Run(() =>
        {
            var result = new List<T>();

            try
            {
                using var workbook = new XLWorkbook(filePath);
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RangeUsed().RowsUsed().ToList();

                if (rows.Count == 0) return result;

                var headers = rows[0].Cells()
                    .Select(c => c.Value.ToString())
                    .ToList();

                var properties = typeof(T).GetProperties();

                for (int i = 1; i < rows.Count; i++)
                {
                    var row = rows[i];
                    var item = new T();
                    var cells = row.Cells().ToList();

                    for (int j = 0; j < headers.Count && j < cells.Count; j++)
                    {
                        var property = properties.FirstOrDefault(p =>
                            p.Name.Equals(headers[j], StringComparison.OrdinalIgnoreCase));

                        if (property != null && property.CanWrite)
                        {
                            try
                            {
                                var value = Convert.ChangeType(cells[j].Value, property.PropertyType);
                                property.SetValue(item, value);
                            }
                            catch
                            {
                                // Ignorer les erreurs de conversion
                            }
                        }
                    }

                    result.Add(item);
                }

                _logger.LogInformation($"Successfully read {result.Count} items from {filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error reading Excel file: {filePath}");
                throw;
            }

            return result;
        });
    }

    public async Task<byte[]> ConvertExcelToImageAsync(string filePath, string worksheetName = "")
    {
        // Cette fonctionnalité nécessite PuppeteerSharp pour convertir HTML en image
        var html = await GenerateHtmlFromExcelAsync(filePath, worksheetName);
        
        // TODO: Implémenter la conversion HTML -> Image avec PuppeteerSharp
        _logger.LogWarning("Excel to Image conversion not yet implemented");
        
        return Array.Empty<byte>();
    }

    public async Task<string> GenerateHtmlFromExcelAsync(string filePath, string worksheetName = "")
    {
        return await Task.Run(() =>
        {
            try
            {
                using var workbook = new XLWorkbook(filePath);
                var worksheet = string.IsNullOrEmpty(worksheetName)
                    ? workbook.Worksheet(1)
                    : workbook.Worksheet(worksheetName);

                var html = "<table border='1' style='border-collapse: collapse;'>";

                foreach (var row in worksheet.RowsUsed())
                {
                    html += "<tr>";
                    foreach (var cell in row.CellsUsed())
                    {
                        var bgColor = cell.Style.Fill.BackgroundColor.Color.Name;
                        var style = $"padding: 5px; background-color: #{bgColor};";
                        html += $"<td style='{style}'>{cell.Value}</td>";
                    }
                    html += "</tr>";
                }

                html += "</table>";

                return html;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error generating HTML from Excel: {filePath}");
                throw;
            }
        });
    }
}

