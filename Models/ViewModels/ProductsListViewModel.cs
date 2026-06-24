namespace SportHub.Models.ViewModels;

// the productList view model as a container with combined information about Products to pass inside views that will render products
public class ProductsListViewModel
{
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
    public PageInfoViewModel PageInfo { get; set; } = new();
    public string? CurrentCategory { get; set; }
}