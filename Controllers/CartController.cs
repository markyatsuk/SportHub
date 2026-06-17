using Microsoft.AspNetCore.Mvc;
using SportHub.Infrastructure;
using SportHub.Models;
using SportHub.Models.Repository;
using SportHub.Models.ViewModels;

namespace SportHub.Controllers;

public class CartController(IHubRepository repository, Cart cart) : Controller
{
    private readonly IHubRepository repository = repository ?? throw new ArgumentNullException(nameof(repository));
    
    private Cart Cart { get; set; } = cart ?? throw new ArgumentNullException(nameof(cart));
    
    [HttpGet]
    public IActionResult Index(string returnUrl)
    {
        // return CartViewModel with session data
        return this.View(new CartViewModel
        {
            ReturnUrl = new Uri(returnUrl ?? "/"),
            Cart = this.Cart,
        });
    }
    
    // add POST action for adding items to cart
    [HttpPost]
    public IActionResult Index(long productId, Uri returnUrl)
    {
        Product? product = this.repository.Products.FirstOrDefault(p => p.ProductId == productId);

        if (product != null)
        {
            Cart.AddItem(product, 1);
            return this.View(new CartViewModel { Cart = Cart, ReturnUrl = returnUrl});
        }

        return this.RedirectToAction("Index", "Home");
    }
    
    [HttpPost]
    [Route("Cart/Remove")]
    // Remove action method for cart items
    public IActionResult Remove(long productId, Uri returnUrl)
    {
        var lineToRemove = this.Cart.Lines.FirstOrDefault(cl => cl.Product.ProductId == productId);
        if (lineToRemove != null)
        {
            this.Cart.RemoveLine(lineToRemove.Product);
        }
        
        return this.View("Index", new CartViewModel
        {
            Cart = this.Cart,
            ReturnUrl = returnUrl
        });
    }

}