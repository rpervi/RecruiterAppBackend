using Microsoft.EntityFrameworkCore;
using Recruitment.NotificationService.Models;

namespace Recruitment.NotificationService.Data;

public class NotificationDbContext(DbContextOptions<NotificationDbContext> options) : DbContext(options)
{
    public DbSet<EmailNotification> EmailNotifications => Set<EmailNotification>();
}
