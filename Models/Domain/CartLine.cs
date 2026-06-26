namespace SportHub.Models.Domain;

// class for individual cart items
public class CartLine
{
    public int CartLineId { get; set; }
    public Product Product { get; init; } = new();
    public int Quantity { get; set; }
}