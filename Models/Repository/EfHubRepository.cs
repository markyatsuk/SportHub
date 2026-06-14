namespace SportHub.Models.Repository;

// repository to get access to collection IQueryable<Product> Products. Expression Tree is building and extending by adding .Where(), .OrderBy() etc.
public class EfHubRepository(HubDbContext context) : IHubRepository
{
    public IQueryable<Product> Products => context.Products;
}