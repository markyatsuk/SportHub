using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportHub.Models;

// Product Entity. Become Products table in DB.
public class Product
{
    public long ProductId { get; init; }
    
    [Required(ErrorMessage = "Please enter a product name")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Please enter a description")]
    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
    [Column(TypeName = "decimal(8, 2)")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "Please specify a category")]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;
}