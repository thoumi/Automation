namespace AutomationSystem.Core.Models;

public class FileUpload
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string UploadedBy { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public FileProcessingStatus ProcessingStatus { get; set; }
    public string? ProcessingMessage { get; set; }
}

public enum FileProcessingStatus
{
    Pending,
    Processing,
    Completed,
    Failed
}

