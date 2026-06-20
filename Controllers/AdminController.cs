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