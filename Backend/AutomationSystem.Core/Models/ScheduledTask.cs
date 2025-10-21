namespace AutomationSystem.Core.Models;

public class ScheduledTask
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Planification simplifiée (stockée en JSON)
    public string? ScheduleJson { get; set; }
    
    // Expression CRON (générée automatiquement ou personnalisée)
    public string CronExpression { get; set; } = string.Empty;
    
    public bool IsEnabled { get; set; }
    public string TaskType { get; set; } = string.Empty;
    public string? Configuration { get; set; }
    public DateTime? LastExecutionTime { get; set; }
    public DateTime? NextExecutionTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

