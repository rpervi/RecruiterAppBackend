namespace Recruitment.NotificationService.Models;

public class EmailNotification
{
    public int Id { get; set; }
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Status { get; set; } = "Queued";
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
