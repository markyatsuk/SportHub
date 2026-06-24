namespace SportHub.Models.Repository;

// repository to get access to collection IQueryable<Product> Products and to manipulate date in dbContext by CRUD methods. Expression Tree is building and extending by adding .Where(), .OrderBy() etc.
public class EfHubRepository(HubDbContext context) : IHubRepository
{
    public IQueryable<Product> Products => context.Products;
    
    // add a new product to the dbContext. save changes
    public void CreateProduct(Product product)
    {
        context.Add(product);
        context.SaveChanges();
    }

    // update product in the dbContext. save changes
    public void UpdateProduct(Product product)
    {
            Product? dbEntry = context.Products?.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (dbEntry == null) return;
        
                dbEntry.Name = product.Name;
                dbEntry.Description = product.Description;
                dbEntry.Price = product.Price;
                dbEntry.Category = product.Category;
                
        // request to db for saving changes is only if dbEntry exists 
            context.SaveChanges();
    }
    
    // delete a product from the dbContext. save changes
    public void DeleteProduct(Product product)
    {
        context.Remove(product);
        context.SaveChanges();
    }
}