namespace AutomationSystem.Core.Interfaces;

public interface IExcelService
{
    Task<List<Dictionary<string, object>>> ReadExcelFileAsync(string filePath);
    Task<List<T>> ReadExcelFileAsync<T>(string filePath) where T : class, new();
    Task<byte[]> ConvertExcelToImageAsync(string filePath, string worksheetName = "");
    Task<string> GenerateHtmlFromExcelAsync(string filePath, string worksheetName = "");
}

