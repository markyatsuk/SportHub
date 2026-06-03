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
                Name = "Trail Running Shoes",
                Description = "Lightweight shoes with aggressive grip for off-road terrain",
                Category = "Running",
                Price = 129.99m,
            },
            new Product
            {
                Name = "GPS Running Watch",
                Description = "Track your pace, distance, and heart rate in real time",
                Category = "Running",
                Price = 249.95m,
            },
            new Product
            {
                Name = "Compression Socks",
                Description = "Reduce muscle fatigue on long distance runs",
                Category = "Running",
                Price = 18.50m,
            },
            new Product
            {
                Name = "Foam Roller",
                Description = "Deep tissue massage for post-workout recovery",
                Category = "Recovery",
                Price = 34.99m,
            },
            new Product
            {
                Name = "Ice Bath Tub",
                Description = "Portable cold therapy tub for serious athletes",
                Category = "Recovery",
                Price = 189.00m,
            },
            new Product
            {
                Name = "Resistance Band Set",
                Description = "Five levels of resistance for strength and mobility training",
                Category = "Fitness",
                Price = 27.95m,
            },
            new Product
            {
                Name = "Adjustable Dumbbell",
                Description = "Replaces 15 sets of weights in one compact design",
                Category = "Fitness",
                Price = 349.00m,
            },
            new Product
            {
                Name = "Pull-Up Bar",
                Description = "Doorframe mounted bar supporting up to 150kg",
                Category = "Fitness",
                Price = 44.95m,
            },
            new Product
            {
                Name = "Yoga Mat Pro",
                Description = "Extra thick non-slip mat with alignment lines",
                Category = "Yoga",
                Price = 59.99m,
            },
            new Product
            {
                Name = "Yoga Blocks Set",
                Description = "Pair of cork blocks for improved pose stability",
                Category = "Yoga",
                Price = 22.00m,
            },
            new Product
            {
                Name = "Meditation Cushion",
                Description = "Ergonomic zafu cushion for long sitting sessions",
                Category = "Yoga",
                Price = 38.50m,
            },
            new Product
            {
                Name = "Road Bicycle",
                Description = "Carbon frame road bike built for speed and endurance",
                Category = "Cycling",
                Price = 1299.00m,
            },
            new Product
            {
                Name = "Cycling Helmet",
                Description = "Aerodynamic helmet with MIPS safety technology",
                Category = "Cycling",
                Price = 89.95m,
            },
            new Product
            {
                Name = "Bike Repair Kit",
                Description = "Everything you need to fix a flat on the road",
                Category = "Cycling",
                Price = 15.99m,
            },
            new Product
            {
                Name = "Basketball",
                Description = "Official size and weight indoor/outdoor ball",
                Category = "Basketball",
                Price = 39.95m,
            },
            new Product
            {
                Name = "Portable Basketball Hoop",
                Description = "Height-adjustable hoop with weighted base",
                Category = "Basketball",
                Price = 229.00m,
            },
            new Product
            {
                Name = "Tennis Racket",
                Description = "Lightweight graphite racket for intermediate players",
                Category = "Tennis",
                Price = 74.99m,
            },
            new Product
            {
                Name = "Tennis Ball Tube",
                Description = "Pack of four pressurized match-quality balls",
                Category = "Tennis",
                Price = 8.95m,
            },
            new Product
            {
                Name = "Swimming Goggles",
                Description = "Anti-fog UV protection goggles for competitive swimmers",
                Category = "Swimming",
                Price = 29.99m,
            },
            new Product
            {
                Name = "Swim Training Fins",
                Description = "Short blade fins to build ankle strength and kick technique",
                Category = "Swimming",
                Price = 45.00m,
            });
            
            context.SaveChanges();
        }
    }
}