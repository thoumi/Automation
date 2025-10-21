namespace AutomationSystem.Core.Interfaces;

public interface IWhatsAppService
{
    Task<bool> SendTextMessageAsync(string phoneNumber, string message);
    Task<bool> SendImageMessageAsync(string phoneNumber, byte[] imageData, string caption = "");
    Task<bool> SendDocumentAsync(string phoneNumber, byte[] documentData, string fileName);
    Task<List<WhatsAppRecipient>> GetRecipientsAsync();
}

public class WhatsAppRecipient
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

