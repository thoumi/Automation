namespace AutomationSystem.Core.Models;

public class TaskExecutionLog
{
    public int Id { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public DateTime ExecutionTime { get; set; }
    public TaskStatus Status { get; set; }
    public string? Message { get; set; }
    public string? ErrorDetails { get; set; }
    public int DurationMs { get; set; }
}

public enum TaskStatus
{
    Success,
    Failed,
    Warning,
    Running
}

