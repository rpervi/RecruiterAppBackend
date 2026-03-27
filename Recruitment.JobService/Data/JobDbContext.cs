using Microsoft.EntityFrameworkCore;
using Recruitment.JobService.Models;

namespace Recruitment.JobService.Data;

public class JobDbContext(DbContextOptions<JobDbContext> options) : DbContext(options)
{
    public DbSet<Job> Jobs => Set<Job>();
}
