namespace SportHub.Models.Repository;

public class EfHubRepository(HubDbContext context) : IHubRepository
{
    public IQueryable<Product> Products => context.Products;
}