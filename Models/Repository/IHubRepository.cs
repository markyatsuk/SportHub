using SportHub.Models.Domain;

namespace SportHub.Models.Repository;

// defines what EfHubRepository will do
public interface IHubRepository
{
    IQueryable<Product> Products { get; }
    
    void CreateProduct(Product p);
    
    void UpdateProduct(Product p);
    
    void DeleteProduct(Product p);
}