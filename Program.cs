using System.Globalization;
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

var app = builder.Build();

var supportedCultures = new[] { new CultureInfo("en-US") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

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
    "checkout",
    "checkout",
    new { Controller = "Order", action = "Checkout" });

app.MapControllerRoute(
    "remove",
    "Remove",
    new { Controller = "Cart", action = "Remove" });


// Ensuring our database has data. Passing app object as a parameter to create scope in SeedData class
SeedData.EnsurePopulated(app);

app.Run();
