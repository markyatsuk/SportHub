namespace SportHub.Models.Domain;

// Determine our Cart class logic. Basic class does not include session work. 
public class Cart
{
    // [JsonInclude] - is redundant here. System.Text.Json does not change lists, it adds new items by .Add() method. So we need [JsonInclude] attribute when we have properties with private setter and it is not Collection with .Add() method.
    public List<CartLine> Lines { get; } = [];

    // logic of adding product to our cart
    public virtual void AddItem(Product product, int quantity)
    {
        // check if we already have this item(that we want to add) in our cart
        CartLine? line = Lines
            .FirstOrDefault(p => p.Product.ProductId == product.ProductId);
        // add new product to our cart collection
        if (line is null)
        {
            Lines.Add(new CartLine
            {
                Product = product,
                Quantity = quantity,
            });
        }
        // just increment quantity if we already have this product in our cart
        else
        {
            line.Quantity += quantity;
        }
    }
    
    // logic of deleting product from the cart
    public virtual void RemoveLine(Product product)
        => Lines.RemoveAll(l => l.Product.ProductId == product.ProductId);
    
    // logic of computing total cart's value
    public decimal ComputeTotalValue()
        => Lines.Sum(e => e.Product.Price * e.Quantity);
    
    // logic of clearing of the cart
    public virtual void Clear() => Lines.Clear();
}