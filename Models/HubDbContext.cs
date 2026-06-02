using Microsoft.EntityFrameworkCore;

namespace SportHub.Models;

// Database context - receives a connection configuration via DI
public class HubDbContext(DbContextOptions<HubDbContext> options) : DbContext(options)
{
    // Read-only access to Products table
    public DbSet<Product> Products => Set<Product>();
}