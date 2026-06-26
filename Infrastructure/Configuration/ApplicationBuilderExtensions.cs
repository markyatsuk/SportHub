using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace SportHub.Infrastructure.Configuration;

// Extension methods for WebApplication to organize middleware pipeline configuration outside Program.cs
public static class ApplicationBuilderExtensions
{
    // Configures middleware pipeline: localization, error handling, routing, auth, static files, session
    public static IApplicationBuilder UseApplicationMiddleware(this WebApplication app)
    {
        // Configure localization to en-US for consistent currency and date formatting
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

        return app;
    }
}