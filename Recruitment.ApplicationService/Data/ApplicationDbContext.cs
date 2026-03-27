using Microsoft.EntityFrameworkCore;
using Recruitment.ApplicationService.Models;

namespace Recruitment.ApplicationService.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Application> Applications => Set<Application>();
}
