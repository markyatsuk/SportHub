using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SportHub.Models;
using SportHub.Models.Repository;

namespace SportHub.Controllers;

// HomeController gets collection IQueryable<Product> Products from IHubRepository via DI
public class HomeController(IHubRepository repository) : Controller
{
    // main method that calls View rendering with collection IQueryable<Product> Products passed as a parameter
    public IActionResult Index()
    {
        return View(repository.Products);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
