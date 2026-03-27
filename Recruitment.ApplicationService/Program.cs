using Microsoft.EntityFrameworkCore;
using Recruitment.ApplicationService.Data;
using Recruitment.ApplicationService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

app.MapGet("/health", () =>
{
    var connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? "not-set";
    return Results.Ok(new
    {
        Service = "Recruitment.ApplicationService",
        Status = "Healthy",
        DatabaseConfigured = connection != "not-set"
    });
});

app.MapGet("/api/applications", async (ApplicationDbContext db) =>
{
    var applications = await db.Applications
        .OrderByDescending(a => a.Id)
        .ToListAsync();
    return Results.Ok(applications);
}).WithName("GetApplications");

app.MapPost("/api/applications", async (ApplicationDbContext db, ApplyRequest request) =>
{
    var application = new Application
    {
        CandidateName = request.CandidateName,
        CandidateEmail = request.CandidateEmail,
        JobId = request.JobId,
        Status = "Submitted"
    };
    db.Applications.Add(application);
    await db.SaveChangesAsync();
    return Results.Created($"/api/applications/{application.Id}", application);
})
.WithName("SubmitApplication");

app.Run();

record ApplyRequest(string CandidateName, string CandidateEmail, int JobId);
