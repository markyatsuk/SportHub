using System.Text.Json;
using System.Text.Json.Serialization;

namespace SportHub.Models;

public class SessionCart : Cart
{
    public static Cart GetCart(IServiceProvider serviceProvider)
    {
        ISession? session = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;

        SessionCart cart = new SessionCart
        {
            Session = session,
        };
        
        // Load cart data from session
        var cartData = session?.GetString("Cart");
        if (!string.IsNullOrEmpty(cartData))
        {
            var deserealizedLines = JsonSerializer.Deserialize<List<CartLine>>(cartData);
            if (deserealizedLines is not null)
            {
                // Load lines directly without triggering SaveCart
                cart.LoadLines(deserealizedLines);
            }
        }
        return cart;
    }
    
    [JsonIgnore]
    private ISession? Session { get; init; }

    public override void AddItem(Product product, int quantity)
    {
        base.AddItem(product, quantity);
        SaveCart();
    }

    public override void RemoveLine(Product product)
    {
        base.RemoveLine(product);
        SaveCart();
    }

    public override void Clear()
    {
        base.Clear();
        Session?.Remove("Cart");
    }

    private void SaveCart()
    {
        if (Session is not null)
        {
            var cartData = JsonSerializer.Serialize(Lines, new JsonSerializerOptions { WriteIndented = true });
            Session.SetString("Cart", cartData);
        }
    }

    private void LoadLines(List<CartLine> lines)
    {
        this.Lines.Clear();
        foreach (var line in lines)
        {
            this.Lines.Add(line);
        }
    }
}