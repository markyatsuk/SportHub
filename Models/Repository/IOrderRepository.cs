using SportHub.Models.Domain;

namespace SportHub.Models.Repository;

// defines what EfOrderRepository will do
public interface IOrderRepository
{
    IQueryable<Order> Orders { get; }
    void SaveOrder(Order order);
}