using Microsoft.EntityFrameworkCore;
using SportHub.Models.Domain;

namespace SportHub.Models.DbContext;

// Database context - receives a connection configuration via DI
public class HubDbContext(DbContextOptions<HubDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    // Read-only access to Products table
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
}