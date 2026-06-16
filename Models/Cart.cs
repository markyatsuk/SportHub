using System.Text.Json.Serialization;

namespace SportHub.Models;

public class Cart
{
    // include private field for serialization explicitly. by default, public members are serialized only
    [JsonInclude]
    private List<CartLine> lines = new();
    [JsonIgnore]
    public IReadOnlyList<CartLine> Lines => lines;
    
    // should have JsonConstructor if we use private fields to instruct serializer how to deserialize data with private fields
    [JsonConstructor]
    public Cart(List<CartLine> lines)
    {
        this.lines = lines ?? new();
    }
    
    public Cart(){}

    public void AddItem(Product product, int quantity)
    {
        CartLine? line = lines
            .FirstOrDefault(p => p.Product.ProductId == product.ProductId);
        if (line is null)
        {
            lines.Add(new CartLine
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
    public void RemoveLine(Product product)
        => lines.RemoveAll(l => l.Product.ProductId == product.ProductId);
    public decimal ComputeTotalValue()
        => lines.Sum(e => e.Product.Price * e.Quantity);
    public void Clear() => lines.Clear();

}