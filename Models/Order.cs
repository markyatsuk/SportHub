using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SportHub.Models;

public class Order
{
    [BindNever]
    public int OrderId { get; init; }

    [BindNever]
    public ICollection<CartLine> Lines { get; } = new List<CartLine>();

    [Required(ErrorMessage = "Please enter a name")]
    [MaxLength(100)]
    public string? Name { get; init; }

    [Required(ErrorMessage = "Please enter the first address line")]
    [MaxLength(100)]
    public string? Line1 { get; init; }

    [MaxLength(100)]
    public string? Line2 { get; init; }
    
    [MaxLength(100)]
    public string? Line3 { get; init; }

    [Required(ErrorMessage = "Please enter a city name")]
    [MaxLength(100)]
    public string? City { get; init; }

    [Required(ErrorMessage = "Please enter a state name")]
    [MaxLength(100)]
    public string? State { get; init; }

    [MaxLength(15)]
    public string? Zip { get; init; }

    [Required(ErrorMessage = "Please enter a country name")]
    [MaxLength(100)]
    public string? Country { get; init; }

    public bool GiftWrap { get; init; }
    
    [BindNever]
    public bool Shipped { get; set; }
    
    public void SetLines(IEnumerable<CartLine> lines)
    {
        Lines.Clear();
        foreach (var line in lines)
        {
            Lines.Add(line);
        }
    }
}
