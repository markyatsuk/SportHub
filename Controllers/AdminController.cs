using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportHub.Models;
using SportHub.Models.Repository;

namespace SportHub.Controllers;

/*
 here Attribute Routing is used. it is suited for:
 - admin panels;
 - API;
 - secured panels.
 usually using simple routes.
 */
[Authorize]
[Route("Admin")]
public class AdminController(IHubRepository hubRepository, IOrderRepository orderRepository) : Controller
{
    // Guard clauses: ensure required dependencies are injected before controller is used
    private readonly IHubRepository _hubRepository = hubRepository ?? throw new ArgumentNullException(nameof(hubRepository));
    private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));

    // GET action method to pass Orders collection to the view
    [Route("Orders")]
    public ViewResult Orders() => View(_orderRepository.Orders);
    
    // GET action method to pass Products collection to the view
    [Route("Products")]
    public ViewResult Products() => View(_hubRepository.Products);
    
    // GET action method with id to pass concrete product to the view
    [Route("Details/{productId:int}")]
    public ViewResult Details(int productId)
        => View(_hubRepository.Products.FirstOrDefault(p => p.ProductId == productId));
    
    // GET action to display edit form and pass concrete product to the view
    [Route("Products/Edit/{productId:long}")]
    public ViewResult Edit(int productId)
    {
        return View(_hubRepository.Products.FirstOrDefault(p => p.ProductId == productId));
    }

    // POST action method to process form submission
    [HttpPost]
    [Route("Products/Edit/{productId:long}")]
    public IActionResult Edit(Product product)
    {
        if (!ModelState.IsValid) return View(product);
        
        // update product if form is filled correct
        _hubRepository.UpdateProduct(product);
        
        // then give control to Products action
        return RedirectToAction("Products");

    }

    // GET action to display create form
    [Route("Products/Create")]
    public ViewResult Create()
    {
        return View(new Product());
    }

    // POST action to process form submission
    [HttpPost]
    [Route("Products/Create")]
    public IActionResult Create(Product product)
    {
        if (!ModelState.IsValid) return View(product);
        
        // create product if form is filled correct
        _hubRepository.CreateProduct(product);
        
        // then give control to Products action
        return RedirectToAction("Products");
    }
    
    // Delete GET action to display delete confirmation
    [Route("Products/Delete/{productId:long}")]
    public IActionResult Delete(int productId)
        => View(_hubRepository.Products.FirstOrDefault(p => p.ProductId == productId));

    // DeleteProduct POST action to process deletion
    [HttpPost]
    [Route("Products/Delete/{productId:long}")]
    public IActionResult DeleteProduct(int productId)
    {
        // find product to delete by id
        var product = _hubRepository.Products.FirstOrDefault(p => p.ProductId == productId);
        
        // delete product from db if exists
        if (product != null) _hubRepository.DeleteProduct(product);
        
        // then give control to Products action
        return RedirectToAction("Products");
    }
    
    // MarkShipped action to mark orders as shipped
    [HttpPost]
    [Route("MarkShipped")]
    public IActionResult MarkShipped(int orderId)
    {
        // find order to mark as shipped by id
        Order? order = orderRepository.Orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order == null) return RedirectToAction("Orders");
        
        // mark order as shipped if exists
        order.Shipped = true;
        
        // update order in db
        orderRepository.SaveOrder(order);
        
        // then give control to Orders action
        return RedirectToAction("Orders");
    }
    
    // Reset action to unmark orders as shipped
    [HttpPost]
    [Route("Reset")]
    public IActionResult Reset(int orderId)
    {
        // find order to mark as shipped by id
        Order? order = orderRepository.Orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order == null) return RedirectToAction("Orders");
        
        // mark order as not shipped if exists
        order.Shipped = false;
        
        // update order in db
        orderRepository.SaveOrder(order);
        
        // then give control to Orders action
        return RedirectToAction("Orders");
    }

}