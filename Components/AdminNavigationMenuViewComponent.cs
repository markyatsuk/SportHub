using Microsoft.AspNetCore.Mvc;

namespace SportHub.Components;

// AdminNavigationMenuViewComponent to fill ViewBag.Selection with category for view purposes
public class AdminNavigationMenuViewComponent : ViewComponent
{
    // array is allocated once on class load, reused on every Invoke() call
    // ReSharper disable once InconsistentNaming
    private static readonly string[] _menuItems = ["Orders", "Products"];
    public IViewComponentResult Invoke()
    {
        ViewBag.Selection = Request.Path.Value ?? "Products";
        return View(_menuItems);
    }
}