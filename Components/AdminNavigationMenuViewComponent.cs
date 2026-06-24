using Microsoft.AspNetCore.Mvc;

namespace SportHub.Components;

// AdminNavigationMenuViewComponent to fill ViewBag.Selection with category for view purposes
public class AdminNavigationMenuViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        ViewBag.Selection = Request.Path.Value ?? "Products";
        return View(new string[] { "Orders", "Products" });
    }
}