using Microsoft.EntityFrameworkCore;
namespace SportHub.Models;

public class SeedData
{
     public static void EnsurePopulated(IApplicationBuilder app)
    {
        // creating scope to have an access to app's services 
        using var scope = app.ApplicationServices.CreateScope();
        // get needed scope from services using our manually created scope
        HubDbContext context = scope.ServiceProvider.GetRequiredService<HubDbContext>();

        // check if it inMemory DB that usually used in Unit Tests(to not work with real DB in tests) && whether it has pending migrations to proceed
        if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory" &&
            context.Database.GetPendingMigrations().Any())
        {
            // proceed pending migrations
            context.Database.Migrate();
        }

        // populate DB with data if there is no data
        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product
                {
                    Name = "Kayak",
                    Description = "A boat for one person",
                    Category = "Watersports",
                    Price = 275,
                },
                new Product
                {
                    Name = "Lifejacket",
                    Description = "Protective and fashionable",
                    Category = "Watersports",
                    Price = 48.95m,
                },
                new Product
                {
                    Name = "Soccer Ball",
                    Description = "FIFA-approved size and weight",
                    Category = "Soccer",
                    Price = 19.50m,
                },
                new Product
                {
                    Name = "Corner Flags",
                    Description = "Give your playing field a professional touch",
                    Category = "Soccer",
                    Price = 34.95m,
                },
                new Product
                {
                    Name = "Stadium",
                    Description = "Flat-packed 35,000-seat stadium",
                    Category = "Soccer",
                    Price = 79500,
                },
                new Product
                {
                    Name = "Thinking Cap",
                    Description = "Improve brain efficiency by 75%",
                    Category = "Chess",
                    Price = 16,
                },
                new Product
                {
                    Name = "Unsteady Chair",
                    Description = "Secretly give your opponent a disadvantage",
                    Category = "Chess",
                    Price = 29.95m,
                },
                new Product
                {
                    Name = "Human Chess Board",
                    Description = "A fun game for the family",
                    Category = "Chess",
                    Price = 75,
                },
                new Product
                {
                    Name = "Bling-Bling King",
                    Description = "Gold-plated, diamond-studded King",
                    Category = "Chess",
                    Price = 1200,
                }
            );
            
            context.SaveChanges();
        }
    }
}