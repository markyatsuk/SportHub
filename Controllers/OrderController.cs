using Microsoft.AspNetCore.Mvc;
using SportHub.Models;
using SportHub.Models.Repository;

namespace SportHub.Controllers;

public class OrderController(IOrderRepository repository, Cart cart) : Controller
{
    public ViewResult Checkout() => View(new Order());
    
    [HttpPost]
    // POST Checkout method for order processing
    public IActionResult Checkout(Order order)
    {
        if (!cart.Lines.Any())
        {
            ModelState.AddModelError("", "Sorry, your cart is empty!");
        }
        if (ModelState.IsValid)
        {
            order.SetLines(cart.Lines);
            repository.SaveOrder(order);
            cart.Clear();
            return View(viewName:"Completed", model: order.OrderId);
        }
        
        return View();
    }

}