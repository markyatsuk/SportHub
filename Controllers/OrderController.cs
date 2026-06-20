using Microsoft.AspNetCore.Mvc;
using SportHub.Models;
using SportHub.Models.Repository;

namespace SportHub.Controllers;

public class OrderController(IOrderRepository repository, Cart cart) : Controller
{
    private readonly IOrderRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    
    private Cart Cart { get; } = cart ?? throw new ArgumentNullException(nameof(cart));
    
    public ViewResult Checkout() => View(new Order());
    
    [HttpPost]
    // POST Checkout method for order processing
    public IActionResult Checkout(Order order)
    {
        if (!Cart.Lines.Any())
        {
            ModelState.AddModelError("", "Sorry, your cart is empty!");
        }
        if (ModelState.IsValid)
        {
            order.SetLines(Cart.Lines);
            _repository.SaveOrder(order);
            Cart.Clear();
            return View(viewName:"Completed", model: order.OrderId);
        }
        
        return View();
    }

}