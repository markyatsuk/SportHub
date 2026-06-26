using System.Text.Json.Serialization;
using SportHub.Infrastructure.Extensions;

namespace SportHub.Models;

// Determine our Session Cart class including session logic
public class SessionCart : Cart
{
    // logic of getting cart object from the session
    public static Cart GetCart(IServiceProvider serviceProvider)
    {
        // get session object from httpContext by IHttpContextAccessor. 
        /* There are two types of services:
         1. DI-service: services that registered through builder.Services.Add*(). To resolve: GetRequiredService<T>() or constructor injection
         2. HttpContext: objects that are related to concrete http request. To access: HttpContext.Session, HttpContext.User, HttpContext.Request, HttpContext.Response.
         ISession is a concrete http request object, which is created and put into HttpContext by middleware.
         In controllers, we have direct access to HttpContext. But in other parts of program (e.g. services) we use IHttpContextAccessor - it's a DI-service that has bridge to the HttpContext.
         So to use this DI-service as a bridge, we should register it in Program.cs: builder.Services.AddHttpContextAccessor();
         */
        
        ISession? session = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;

        // write ISession reference into Session property
        SessionCart cart = new SessionCart
        {
            Session = session,
        };
        
        // Load cart data from session by key "Cart" and deserialize JSON cart data into List<CartLine> type using SessionExtension
        var deserializedLines = session?.GetJson<List<CartLine>>("Cart");
            if (deserializedLines is not null)
            {
                // Load lines directly without triggering SaveCart
                cart.LoadLines(deserializedLines);
            }
        return cart;
    }
    
    [JsonIgnore] // [JsonIgnore] attribute is redundant here. But will leave it for clear documentation of intent
    private ISession? Session { get; init; }

    // overring base AddItem method to add logic of saving cart in session
    public override void AddItem(Product product, int quantity)
    {
        base.AddItem(product, quantity);
        SaveCart();
    }

    // overring base AddItem method to add logic of saving cart in session
    public override void RemoveLine(Product product)
    {
        base.RemoveLine(product);
        SaveCart();
    }

    // overring base AddItem method to add logic of removing cart in session by key "Cart"
    public override void Clear()
    {
        base.Clear();
        Session?.Remove("Cart");
    }

    // logic of saving cart in session
    private void SaveCart()
    {
        // serialize Lines and save to session under "Cart" key using SessionExtension
        Session?.SetJson("Cart", Lines);
    }

    // logic of loading products from session cart to base class Cart
    private void LoadLines(List<CartLine> lines)
    {
        this.Lines.Clear();
        this.Lines.AddRange(lines); // same as:
        /* foreach (var line in lines)
        {
            this.Lines.Add(line);
        } */
    }
}