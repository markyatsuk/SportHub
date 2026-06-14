namespace SportHub.Models.ViewModels;

// the model as a container with combined information about Products
public class ProductsListViewModel
{
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
    public PageInfoViewModel PageInfo { get; set; } = new();
    public string? CurrentCategory { get; set; }
}