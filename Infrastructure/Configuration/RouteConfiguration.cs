namespace SportHub.Infrastructure.Configuration;

// Extension methods for IEndpointRouteBuilder to organize all application routes outside Program.cs
public static class RouteConfiguration
{
    // Maps all application routes: pagination, category filtering, cart, checkout, and utility routes
    public static IEndpointRouteBuilder MapApplicationRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllerRoute(
            name: "pagination",
            pattern: "Products/Page{productPage:int}",
            defaults: new { Controller = "Home", action = "Index", productPage = 1 });

        endpoints.MapControllerRoute(
            name: "categoryPage",
            pattern: "{category}/Page{productPage:int}",
            defaults: new { Controller = "Home", action = "Index" });

        endpoints.MapControllerRoute(
            name: "category",
            pattern: "Products/{category}",
            defaults: new { Controller = "Home", action = "Index", productPage = 1 });

        endpoints.MapControllerRoute(
            name: "shoppingCart",
            pattern: "Cart",
            defaults: new { Controller = "Cart", action = "Index" });

        endpoints.MapControllerRoute(
            name: "default",
            pattern: "/",
            defaults: new { Controller = "Home", action = "Index" }); 

        endpoints.MapControllerRoute(
            name: "checkout",
            pattern: "checkout",
            new { Controller = "Order", action = "Checkout" });

        endpoints.MapControllerRoute(
            name: "remove",
            pattern: "Remove",
            defaults: new { Controller = "Cart", action = "Remove" });

        endpoints.MapControllerRoute(
            name: "error",
            pattern: "Error",
            defaults: new { Controller = "Home", action = "Error" });
        
        return endpoints;
    }
}