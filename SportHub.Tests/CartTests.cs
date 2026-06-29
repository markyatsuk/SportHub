using SportHub.Models.Domain;

namespace SportHub.Tests;

public class CartTests
{
    [Fact]
    public void AddItem_NewProduct_AddsLineToCart()
    {
        // Arrange
        var cart = new Cart();
        var product = new Product { ProductId = 1, Name = "Nike Shoes", Price = 99.99m };
        
        // Act
        cart.AddItem(product, 2);

        // Assert
        Assert.Single(cart.Lines);
    }
    
    [Fact]
    public void AddItem_NewProduct_AddsProductsToLineToCart()
    {
        // Arrange
        var cart = new Cart();
        var product = new Product { ProductId = 1, Name = "Nike Shoes", Price = 99.99m };
        
        // Act
        cart.AddItem(product, 2);

        // Assert
        Assert.Single(cart.Lines);
        Assert.Equal(2, cart.Lines[0].Quantity);
    }
    [Fact]
    public void AddItem_ExistingProduct_IncrementsQuantity()
    {
        // Arrange
        var cart = new Cart();
        var product = new Product { ProductId = 1, Name = "Nike Shoes", Price = 99.99m };
       
        // Act
        cart.AddItem(product, 2);
        cart.AddItem(product, 3);
        
        // Assert
        Assert.Single(cart.Lines);
        Assert.Equal(5, cart.Lines[0].Quantity);
    }
    [Fact]
    public void RemoveLine_ExistingProduct_DeletesLineFromCart()
    {
        // Arrange
        var cart = new Cart();
        var product = new Product { ProductId = 1, Name = "Nike Shoes", Price = 99.99m };
        cart.AddItem(product, 2);
        
        // Act
        cart.RemoveLine(product);
        
        // Assert
        Assert.Empty(cart.Lines); 
    }
    [Fact]
    public void ComputeTotalValue_MultipleProducts_ReturnsSumOfAllLines()
    {
        // Arrange
        var cart = new Cart();
        var product1 = new Product { ProductId = 1, Name = "Nike Shoes", Price = 99.99m };
        var product2 = new Product { ProductId = 2, Name = "Nike Bag", Price = 35.99m };
        cart.AddItem(product1, 1);
        cart.AddItem(product2, 1);
        
        // Act
        var sum = cart.ComputeTotalValue();
        
        // Assert
        Assert.Equal(product1.Price + product2.Price, sum); 
    }
    [Fact]
    public void ComputeTotalValue_ProductWithQuantity_MultipliesPriceByQuantity()
    {
        // Arrange
        var cart = new Cart();
        var product = new Product { ProductId = 1, Name = "Nike Shoes", Price = 99.99m };
        cart.AddItem(product, 3);
    
        // Act
        var sum = cart.ComputeTotalValue();
    
        // Assert
        Assert.Equal(product.Price * 3, sum);
    }
    [Fact]
    public void Clear_CartWithProducts_RemovesAllLines()
    {
        // Arrange
        var cart = new Cart();
        var product1 = new Product { ProductId = 1, Name = "Nike Shoes", Price = 99.99m };
        var product2 = new Product { ProductId = 2, Name = "Nike Bag", Price = 35.99m };
        cart.AddItem(product1, 2);
        cart.AddItem(product2, 3);
        
        // Act
        cart.Clear();
        
        // Assert
        Assert.Empty(cart.Lines); 
    }
    [Fact]
    public void AddItem_MultipleProducts_AddsLinesToCart()
    {
        // Arrange
        var cart = new Cart();
        var product1 = new Product { ProductId = 1, Name = "Nike Shoes", Price = 99.99m };
        var product2 = new Product { ProductId = 2, Name = "Nike Bag", Price = 35.99m };
        
        // Act
        cart.AddItem(product1, 1);
        cart.AddItem(product2, 1);
        
        // Assert
        Assert.Equal(2, cart.Lines.Count); 
    }
    [Fact]
    public void RemoveLine_NonExistingProduct_DoesNotThrow()
    {
        // Arrange
        var cart = new Cart();
        var product = new Product { ProductId = 1, Name = "Nike Shoes", Price = 99.99m };
    
        // Act
        var exception = Record.Exception(() => cart.RemoveLine(product));
        
        // Assert
        Assert.Null(exception);
    }
    [Fact]
    public void ComputeTotalValue_EmptyCart_ReturnsZero()
    {
        // Arrange
        var cart = new Cart();
    
        // Act
        var sum = cart.ComputeTotalValue();
    
        // Assert
        Assert.Equal(0m, sum);
    }
    [Fact]
    public void Clear_EmptyCart_DoesNotThrow()
    {
        // Arrange
        var cart = new Cart();
    
        // Act
        var exception = Record.Exception(cart.Clear);
        
        // Assert
        Assert.Null(exception);
    }
}