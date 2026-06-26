using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportHub.Models;
using SportHub.Models.DbContext;
using SportHub.Models.Domain;
using SportHub.Models.Repository;

namespace SportHub.Infrastructure.Configuration;

// Extension methods for IServiceCollection to organize service registration outside Program.cs
public static class ServiceCollectionExtensions
{
    // Registers all application services: MVC, databases, repositories, session, cart, and identity
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add MVC services
        services.AddControllersWithViews();
        
        // Builder service AddDbContext - registers connection to DB in DI
        services.AddDbContext<HubDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("SportHubConnection");
            // Set SQL connection using our connective string
            options.UseSqlServer(connectionString ?? "Server=(localdb)\\MSSQLLocalDB;Database=SportHub;MultipleActiveResulSets=true");
        });
        
        // Register repositories by Type-based approach
        services.AddScoped<IHubRepository, EfHubRepository>();
        services.AddScoped<IOrderRepository, EfOrderRepository>();
        
        // Add distributed memory cache service for session storage and session service itself for cart persistence. Storage is in RAM of a server.
        services.AddDistributedMemoryCache();
        services.AddSession();
        
        // Register Cart service by Factory-based registration. In that case DI says: "when someone needs Cart - do not create it by yourself but call that function"
        services.AddScoped<Cart>(SessionCart.GetCart);
        
        // Register HttpContextAccessor for session access by extension-method
        /* Is the same as builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        under the hood it is doing:
        public static IServiceCollection AddHttpContextAccessor(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }

        the only difference:
        AddSingleton(...)    - registers always. even it exists - will replace.
        TryAddSingleton(...) - registers only if it is nor registered yet
        this difference is for libraries and frameworks mostly
        */
        services.AddHttpContextAccessor();

        // Configure Identity database context and services with Entity Framework stores
        services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(configuration["ConnectionStrings:IdentityConnection"]));
        services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
        
        return services;
    }
}