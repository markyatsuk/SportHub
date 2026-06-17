using System.Text.Json.Serialization;

namespace SportHub.Models;

public class Cart
{
    [JsonInclude]
    public List<CartLine> Lines { get; private set; } = new();

    public virtual void AddItem(Product product, int quantity)
    {
        CartLine? line = Lines
            .FirstOrDefault(p => p.Product.ProductId == product.ProductId);
        if (line is null)
        {
            Lines.Add(new CartLine
            {
                Product = product,
                Quantity = quantity,
            });
        }
        else
        {
            line.Quantity += quantity;
        }
    }
    public virtual void RemoveLine(Product product)
        => Lines.RemoveAll(l => l.Product.ProductId == product.ProductId);
    public decimal ComputeTotalValue()
        => Lines.Sum(e => e.Product.Price * e.Quantity);
    public virtual void Clear() => Lines.Clear();

}