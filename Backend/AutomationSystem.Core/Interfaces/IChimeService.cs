namespace AutomationSystem.Core.Interfaces;

public interface IChimeService
{
    Task<bool> SendMessageAsync(string message);
    Task<bool> SendMessageWithImageAsync(string message, byte[] imageData);
    Task<bool> SendAlertAsync(string title, string message, AlertLevel level);
}

public enum AlertLevel
{
    Info,
    Warning,
    Error,
    Critical
}

