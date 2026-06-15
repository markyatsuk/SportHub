using Microsoft.AspNetCore.Mvc;
using SportHub.Infrastructure;
using SportHub.Models;
using SportHub.Models.Repository;
using SportHub.Models.ViewModels;

namespace SportHub.Controllers;

public class CartController(IHubRepository repository) : Controller
{
    private readonly IHubRepository repository = repository ?? throw new ArgumentNullException(nameof(repository));
    [HttpGet]
    public IActionResult Index(string returnUrl)
    {
        // return CartViewModel with session data
        return this.View(new CartViewModel
        {
            ReturnUrl = new Uri(returnUrl ?? "/"),
            Cart = this.HttpContext.Session.GetJson<Cart>("cart") ?? new Cart(),
        });
    }
    [HttpPost]
    // add POST action for adding items to cart
    public IActionResult Index(long productId, Uri returnUrl)
    {
        Product? product = this.repository.Products.FirstOrDefault(p => p.ProductId == productId);

        if (product != null)
        {
            var cart = this.HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            cart.AddItem(product, 1);
            this.HttpContext.Session.SetJson("cart", cart);
            return this.View(new CartViewModel { Cart = cart, ReturnUrl = returnUrl ?? new Uri("/") });
        }

        return this.RedirectToAction("Index", "Home");
    }

}