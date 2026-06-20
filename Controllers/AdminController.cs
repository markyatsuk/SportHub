using Microsoft.AspNetCore.Mvc;
using SportHub.Models;
using SportHub.Models.Repository;

namespace SportHub.Controllers;

[Route("Admin")]
public class AdminController(IHubRepository hubRepository, IOrderRepository orderRepository) : Controller
{
    private readonly IHubRepository _hubRepository = hubRepository ?? throw new ArgumentNullException(nameof(hubRepository));
    private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));

    [Route("Orders")]
    public ViewResult Orders() => View(_orderRepository.Orders);
    
    [Route("Products")]
    public ViewResult Products() => View(_hubRepository.Products);
    
    [Route("Details/{productId:int}")]
    public ViewResult Details(int productId)
        => View(_hubRepository.Products.FirstOrDefault(p => p.ProductId == productId));
    
    [Route("Products/Edit/{productId:long}")]
    // An Edit GET action to display edit form
    public ViewResult Edit(int productId)
    {
        return View(_hubRepository.Products.FirstOrDefault(p => p.ProductId == productId));
    }

    [HttpPost]
    [Route("Products/Edit/{productId:long}")]
    // An Edit POST action to process form submission
    public IActionResult Edit(Product product)
    {
        if (ModelState.IsValid)
        {
            _hubRepository.SaveProduct(product);
            return RedirectToAction("Products");
        }

        return View(product);
    }

    [Route("Products/Create")]
    // GET action to display create form
    public ViewResult Create()
    {
        return View(new Product());
    }

    [HttpPost]
    [Route("Products/Create")]
    // POST action to process form submission
    public IActionResult Create(Product product)
    {
        if (ModelState.IsValid)
        {
            _hubRepository.SaveProduct(product);
            return RedirectToAction("Products");
        }

        return View(product);
    }
    
    [Route("Products/Delete/{productId:long}")]
    // Delete GET action to display delete confirmation
    public IActionResult Delete(int productId)
        => View(_hubRepository.Products.FirstOrDefault(p => p.ProductId == productId));

    [HttpPost]
    [Route("Products/Delete/{productId:long}")]
    //  DeleteProduct POST action to process deletion
    public IActionResult DeleteProduct(int productId)
    {
        var product = _hubRepository.Products.FirstOrDefault(p => p.ProductId == productId);
        if (product != null) _hubRepository.DeleteProduct(product);
        return RedirectToAction("Products");
    }
    
    [HttpPost]
    [Route("MarkShipped")]
    // MarkShipped action to mark orders as shipped
    public IActionResult MarkShipped(int orderId)
    {
        Order? order = orderRepository.Orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order != null)
        {
            order.Shipped = true;
            orderRepository.SaveOrder(order);
        }
        return RedirectToAction("Orders");
    }

    [HttpPost]
    [Route("Reset")]
    // Reset action to unmark orders as shipped
    public IActionResult Reset(int orderId)
    {
        Order? order = orderRepository.Orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order != null)
        {
            order.Shipped = false;
            orderRepository.SaveOrder(order);
        }
        return RedirectToAction("Orders");
    }

}