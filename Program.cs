using SportHub.Infrastructure.Configuration;
using SportHub.Models;
using SportHub.Models.DbContext;

var builder = WebApplication.CreateBuilder(args);

// Extension for IServiceCollection to organize service registration
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Extension for WebApplication to organize middleware pipeline configuration
app.UseApplicationMiddleware();

// Extension for IEndpointRouteBuilder to organize all application routes
app.MapApplicationRoutes();

// Ensuring our database has data. Passing app object as a parameter to create scope in SeedData class
SeedData.EnsurePopulated(app);

// Seed Identity database with admin user
await IdentitySeedData.EnsurePopulated(app);

app.Run();