namespace SportHub.Models.Repository;

public interface IHubRepository
{
    IQueryable<Product> Products { get; }
}