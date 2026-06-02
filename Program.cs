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

//??
builder.Services.AddScoped<IHubRepository, EfHubRepository>();

var app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ??
SeedData.EnsurePopulated(app);

app.Run();
