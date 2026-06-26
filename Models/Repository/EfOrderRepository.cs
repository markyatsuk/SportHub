using Microsoft.EntityFrameworkCore;
using SportHub.Models.DbContext;
using SportHub.Models.Domain;

namespace SportHub.Models.Repository;

// repository for reading and saving Orders with their related Lines and Products
public class EfOrderRepository(HubDbContext context) : IOrderRepository
{
    // get joined tables using EF eager loading
    public IQueryable<Order> Orders => context.Orders.Include(o => o.Lines)
        .ThenInclude(l => l.Product);
    
    public void SaveOrder(Order order)
    {
        // attachRange says which products already exist to not try to create duplicates
        context.AttachRange(order.Lines.Select(l => l.Product));
        // OrderId == 0 means new order (not yet persisted) — only insert, never update existing orders
        if (order.OrderId == 0)
        {
            // add new order to db
            context.Orders.Add(order);
        }
        // save changes
        context.SaveChanges();
    }
}