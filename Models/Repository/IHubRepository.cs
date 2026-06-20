namespace SportHub.Models.Repository;

public interface IHubRepository
{
    IQueryable<Product> Products { get; }
    void SaveProduct(Product p);
    
    void CreateProduct(Product p);
    
    void DeleteProduct(Product p);

}