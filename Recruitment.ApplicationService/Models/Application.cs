namespace Recruitment.ApplicationService.Models;

public class Application
{
    public int Id { get; set; }
    public string CandidateName { get; set; } = string.Empty;
    public string CandidateEmail { get; set; } = string.Empty;
    public int JobId { get; set; }
    public string Status { get; set; } = "Submitted";
    public DateTime AppliedAtUtc { get; set; } = DateTime.UtcNow;
}
