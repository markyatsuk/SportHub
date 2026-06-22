using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using SportHub.Models;
using SportHub.Models.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Builder service AddDbContext - registers connection to DB in DI
builder.Services.AddDbContext<HubDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("SportHubConnection");
    // Set SQL connection using our connective string
    options.UseSqlServer(connectionString ?? "Server=(localdb)\\MSSQLLocalDB;Database=SportHub;MultipleActiveResulSets=true");
});

// Register EfHubRepository that return collection of IQueryable<Product> Products in DI
builder.Services.AddScoped<IHubRepository, EfHubRepository>();

// Register Order repository service for dependency injection
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();

// Add distributed memory cache service for session storage. Storage is in RAM of a server.
builder.Services.AddDistributedMemoryCache();

// Add session service for cart persistence
builder.Services.AddSession();

// Register Cart service with dependency injection
builder.Services.AddScoped<Cart>(SessionCart.GetCart);

// Register HttpContextAccessor for session access
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Configure Identity database context
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:IdentityConnection"]));

// Configure Identity services with Entity Framework stores
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();

var app = builder.Build();

var supportedCultures = new[] { new CultureInfo("en-US") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// Production environment check for error handling
if (!app.Environment.IsDevelopment())
{
    // Configure custom error handler for production environment
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

// Enable session middleware
app.UseSession();

app.MapControllerRoute(
    name: "pagination",
    pattern: "Products/Page{productPage:int}",
    defaults: new { Controller = "Home", action = "Index", productPage = 1 });

app.MapControllerRoute(
    name: "categoryPage",
    pattern: "{category}/Page{productPage:int}",
    defaults: new { Controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "category",
    pattern: "Products/{category}",
    defaults: new { Controller = "Home", action = "Index", productPage = 1 });

app.MapControllerRoute(
    name: "shoppingCart",
    pattern: "Cart",
    defaults: new { Controller = "Cart", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "/",
    defaults: new { Controller = "Home", action = "Index" }); 

app.MapControllerRoute(
    name: "checkout",
    pattern: "checkout",
    new { Controller = "Order", action = "Checkout" });

app.MapControllerRoute(
    name: "remove",
    pattern: "Remove",
    defaults: new { Controller = "Cart", action = "Remove" });

app.MapControllerRoute(
    name: "error",
    pattern: "Error",
    defaults: new { Controller = "Home", action = "Error" });


// Ensuring our database has data. Passing app object as a parameter to create scope in SeedData class
SeedData.EnsurePopulated(app);

// Seed Identity database with admin user
await IdentitySeedData.EnsurePopulated(app);


app.Run();
