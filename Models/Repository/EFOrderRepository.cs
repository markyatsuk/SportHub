using Microsoft.EntityFrameworkCore;

namespace SportHub.Models.Repository;

public class EfOrderRepository(HubDbContext context) : IOrderRepository
{
    public IQueryable<Order> Orders => context.Orders.Include(o => o.Lines)
        .ThenInclude(l => l.Product);
    public void SaveOrder(Order order)
    {
        context.AttachRange(order.Lines.Select(l => l.Product));
        if (order.OrderId == 0)
        {
            context.Orders.Add(order);
        }
        context.SaveChanges();

    }
}