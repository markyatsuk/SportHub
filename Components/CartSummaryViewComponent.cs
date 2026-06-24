using Microsoft.AspNetCore.Mvc;
using SportHub.Models;

namespace SportHub.Components;

// CartSummaryViewComponent for passing Cart model inside partial view
public class CartSummaryViewComponent(Cart cart) : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View(cart);
    }
}
