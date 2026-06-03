using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SportHub.Models;
using SportHub.Models.Repository;
using SportHub.Models.ViewModels;

namespace SportHub.Controllers;

// HomeController gets collection IQueryable<Product> Products from IHubRepository via DI
public class HomeController(IHubRepository repository) : Controller
{
    private const int PageSize = 5;
    
    // action that passes ProductsListViewModel as a parameter. ProductsListViewModel contains IEnumerable<Product> Products and PageInfo PagingInfo
    public IActionResult Index(int productPage = 1)
    {
        return View(new ProductsListViewModel
        {
            Products = repository.Products.OrderBy(product => product.ProductId).Skip((productPage - 1) * PageSize)
                .Take(PageSize),
            PageInfo =
            {
                TotalItems = repository.Products.Count(),
                ItemsPerPage = PageSize,
                CurrentPage = productPage,
            }
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
