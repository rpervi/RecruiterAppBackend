using Microsoft.EntityFrameworkCore;
using Recruitment.NotificationService.Data;
using Recruitment.NotificationService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
    db.Database.EnsureCreated();
}

app.MapGet("/health", () =>
{
    var connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? "not-set";
    return Results.Ok(new
    {
        Service = "Recruitment.NotificationService",
        Status = "Healthy",
        DatabaseConfigured = connection != "not-set"
    });
});

app.MapGet("/api/notifications/email", async (NotificationDbContext db) =>
{
    var notifications = await db.EmailNotifications
        .OrderByDescending(n => n.Id)
        .ToListAsync();
    return Results.Ok(notifications);
}).WithName("GetEmailNotifications");

app.MapPost("/api/notifications/email", async (NotificationDbContext db, EmailNotificationRequest request) =>
{
    var notification = new EmailNotification
    {
        To = request.To,
        Subject = request.Subject,
        Body = request.Body,
        Status = "Queued"
    };

    db.EmailNotifications.Add(notification);
    await db.SaveChangesAsync();

    return Results.Ok(new
    {
        Message = "Email notification queued",
        notification.Id,
        notification.To,
        notification.Subject,
        notification.Status,
        notification.CreatedAtUtc
    });
}).WithName("QueueEmailNotification");

app.Run();

record EmailNotificationRequest(string To, string Subject, string Body);
