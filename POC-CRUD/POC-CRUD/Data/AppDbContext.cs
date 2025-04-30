using Microsoft.EntityFrameworkCore;
using POC_CRUD.Models;

namespace POC_CRUD.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<HealthStatus> HealthStatuses { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}