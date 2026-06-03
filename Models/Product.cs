using System.ComponentModel.DataAnnotations.Schema;

namespace SportHub.Models;

// Product Entity. Become Products table in DB.
public class Product
{
    public long ProductId { get; init; }
    public string Name { get; set; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Price { get; set; }

    public string Category { get; init; } = string.Empty;

}