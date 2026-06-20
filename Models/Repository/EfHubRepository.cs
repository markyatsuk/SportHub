namespace SportHub.Models.Repository;

// repository to get access to collection IQueryable<Product> Products. Expression Tree is building and extending by adding .Where(), .OrderBy() etc.
public class EfHubRepository(HubDbContext context) : IHubRepository
{
    public IQueryable<Product> Products => context.Products;
    
    public void CreateProduct(Product product)
    {
        context.Add(product);
        context.SaveChanges();
    }

    public void DeleteProduct(Product product)
    {
        context.Remove(product);
        context.SaveChanges();
    }

    public void SaveProduct(Product product)
    {
        if (product.ProductId == 0)
        {
            context.Products.Add(product);
        }
        else
        {
            Product? dbEntry = context.Products?.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (dbEntry != null)
            {
                dbEntry.Name = product.Name;
                dbEntry.Description = product.Description;
                dbEntry.Price = product.Price;
                dbEntry.Category = product.Category;
            }
        }
        context.SaveChanges();
    }

}