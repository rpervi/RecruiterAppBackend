using Microsoft.EntityFrameworkCore;
using Recruitment.JobService.Data;
using Recruitment.JobService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<JobDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<JobDbContext>();
    db.Database.EnsureCreated();
    if (!db.Jobs.Any())
    {
        db.Jobs.AddRange(
            new Job
            {
                Title = "Junior Frontend Developer",
                Company = "Nexcode Systems",
                Location = "Remote, India",
                Stack = "React, TypeScript, REST APIs",
                Description = "Build and maintain frontend modules for hiring workflows."
            },
            new Job
            {
                Title = "Backend Engineer",
                Company = "CloudBridge Tech",
                Location = "Bengaluru",
                Stack = "Node.js, PostgreSQL, Docker",
                Description = "Develop secure APIs and optimize recruitment data services."
            },
            new Job
            {
                Title = "QA Automation Tester",
                Company = "SecureSoft Labs",
                Location = "Hyderabad",
                Stack = "Selenium, Cypress, CI/CD",
                Description = "Create automated regression coverage for recruitment features."
            });
        db.SaveChanges();
    }
}

app.MapGet("/health", () =>
{
    var connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? "not-set";
    return Results.Ok(new
    {
        Service = "Recruitment.JobService",
        Status = "Healthy",
        DatabaseConfigured = connection != "not-set"
    });
});

app.MapGet("/api/jobs", async (JobDbContext db) =>
{
    var jobs = await db.Jobs
        .OrderByDescending(j => j.Id)
        .ToListAsync();
    return Results.Ok(jobs);
})
.WithName("GetJobs");

app.MapPost("/api/jobs", async (JobDbContext db, Job request) =>
{
    db.Jobs.Add(request);
    await db.SaveChangesAsync();
    return Results.Created($"/api/jobs/{request.Id}", request);
}).WithName("CreateJob");

app.Run();
