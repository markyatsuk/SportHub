namespace SportHub.Models.Repository;

// repository to get access to collection IQueryable<Product> Products
public class EfHubRepository(HubDbContext context) : IHubRepository
{
    public IQueryable<Product> Products => context.Products;
}