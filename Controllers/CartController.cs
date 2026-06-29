using Microsoft.AspNetCore.Mvc;
using SportHub.Infrastructure;
using SportHub.Models;
using SportHub.Models.Domain;
using SportHub.Models.Repository;
using SportHub.Models.ViewModels;

namespace SportHub.Controllers;

// CartController gets collection needed services via DI with Primary Constructor
public class CartController(IHubRepository repository, Cart cart) : Controller
{
    // Guard clauses: ensure required dependencies are injected before controller is used
    private readonly IHubRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private Cart Cart { get; } = cart ?? throw new ArgumentNullException(nameof(cart));
    
    // GET action method to pass CartViewModel model to the view to render cart view.
    [HttpGet]
    public IActionResult Index(string returnUrl)
    {
        // return CartViewModel with session data
        return View(new CartViewModel
        {
            ReturnUrl = new Uri(returnUrl, UriKind.Relative),
            Cart = Cart,
        });
    }
    
    // POST action method for adding items to cart
    [HttpPost]
    public IActionResult Index(long productId, Uri returnUrl)
    {
        // find product in db that we want to add to the cart
        Product? product = _repository.Products.FirstOrDefault(p => p.ProductId == productId);
        
        // if there was no project found, just redirect to home page
        if (product == null) return RedirectToAction("Index", "Home");
        
        // call AddItem method on Cart object(Session Cart instance): calling its base method AddItem and call SaveCart to save updated cart in session
        Cart.AddItem(product, 1);
        return View(new CartViewModel { Cart = Cart, ReturnUrl = returnUrl});
    }
    
    // POST action method for removing items from cart 
    [HttpPost]
    [Route("Cart/Remove")]
    public IActionResult Remove(long productId, Uri returnUrl)
    {
        // call RemoveLine method on Cart object(Session Cart instance): calling its base method RemoveLine and call SaveCart to save updated cart in session
        Product? line = Cart.Lines.FirstOrDefault(cl => cl.Product.ProductId == productId)?.Product;
        if (line != null) Cart.RemoveLine(line);
        return View("Index", new CartViewModel
        {
            Cart = Cart,
            ReturnUrl = returnUrl
        });
    }
}