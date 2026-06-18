using Microsoft.AspNetCore.Mvc;
using SportHub.Models;

namespace SportHub.Components;

public class CartSummaryViewComponent(Cart cart) : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View(cart);
    }
}
