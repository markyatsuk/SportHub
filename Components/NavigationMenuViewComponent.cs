using Microsoft.AspNetCore.Mvc;
using SportHub.Models.Repository;

namespace SportHub.Components;

// NavigationMenuViewComponent allows to get IHubRepository via DI to select needed products and pass them inside partial view
public class NavigationMenuViewComponent(IHubRepository repository) : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        ViewBag.SelectedCategory = RouteData?.Values["category"];
        return View(repository.Products
            .Select(x => x.Category)
            .Distinct()
            .OrderBy(x => x));
    }
}
