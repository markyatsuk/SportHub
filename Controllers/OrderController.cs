using Microsoft.AspNetCore.Mvc;
using SportHub.Models;
using SportHub.Models.Domain;
using SportHub.Models.Repository;

namespace SportHub.Controllers;

// OrderController handles checkout flow — requires Cart (session data) and IOrderRepository (persistence) gets via DI with Primary Constructor

public class OrderController(IOrderRepository repository, Cart cart) : Controller
{
    // Guard clauses: ensure required dependencies are injected before controller is used
    private readonly IOrderRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private Cart Cart { get; } = cart ?? throw new ArgumentNullException(nameof(cart));
    
    // GET checkout method. new Order() passed to view, so now view knows about pattern of order
    public ViewResult Checkout() => View(new Order());
    
    // POST Checkout method for order processing
    [HttpPost]
    public IActionResult Checkout(Order order)
    {
        // check if cart is empty
        if (Cart.Lines.Count == 0)
        {
            // show an error message for empty cart
            ModelState.AddModelError("", "Sorry, your cart is empty!");
        }
        // check if form is filled correctly
        if (!ModelState.IsValid) return View(order);
        
        // set products from cart to order object
        order.SetLines(Cart.Lines);
        
        // save order to db
        _repository.SaveOrder(order);
        
        // clear cart
        Cart.Clear();
            
        // return Completed view with passed model as OrderId for confirmation page
        return View(viewName:"Completed", model: order.OrderId);

        /*
         return View(order) — always more correct than return View() because:
            - explicitly passes the model you were working with;
            - retains any data that was set in the controller but did not come from the form;
            - more readable — you can immediately see that the same model is being returned
         */
    }

}