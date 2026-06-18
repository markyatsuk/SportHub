using Microsoft.AspNetCore.Mvc;
using SportHub.Models;

namespace SportHub.Controllers;

public class OrderController : Controller
{
    public ViewResult Checkout() => View(new Order());
}