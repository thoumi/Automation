namespace AutomationSystem.Core.Models;

public class NotificationRecipient
{
    public int Id { get; set; }
    public NotificationType Type { get; set; }
    public string Identifier { get; set; } = string.Empty; // Email, phone number, Chime webhook
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public enum NotificationType
{
    Email,
    WhatsApp,
    Chime
}

