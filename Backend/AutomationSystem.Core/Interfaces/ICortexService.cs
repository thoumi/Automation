namespace AutomationSystem.Core.Interfaces;

public interface ICortexService
{
    Task<List<TourData>> GetToursAsync(DateTime date);
    Task<TourData?> GetTourByIdAsync(string tourId);
    Task<bool> UpdateTourStatusAsync(string tourId, string status);
}

public class TourData
{
    public string TourId { get; set; } = string.Empty;
    public string DriverName { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int StopCount { get; set; }
    public Dictionary<string, object> AdditionalData { get; set; } = new();
}

