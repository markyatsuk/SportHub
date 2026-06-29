using Microsoft.AspNetCore.Mvc;
using Moq;
using SportHub.Controllers;
using SportHub.Models.Domain;
using SportHub.Models.Repository;
using SportHub.Models.ViewModels;

namespace SportHub.Tests;

public class CartControllerTests
{
    private readonly Mock<IHubRepository> _mockRepository;   
    private readonly Cart _cart;
    private readonly CartController _controller;
    private readonly Product _testProduct;
    private readonly Product _testProduct2;
    
    public CartControllerTests()
    {
        // Arrange — shared setup for all tests
        _testProduct = new Product { ProductId = 1, Name = "Nike Shoes", Price = 99.99m };
        _testProduct2 = new Product { ProductId = 2, Name = "Nike Bag", Price = 35.99m };
        
        _mockRepository = new Mock<IHubRepository>();
        _mockRepository.Setup(r => r.Products)
            .Returns(new[] { _testProduct, _testProduct2 }.AsQueryable());
        
        _cart = new Cart();
        _controller = new CartController(_mockRepository.Object, _cart);
    }
    [Fact]
    public void IndexPost_ValidProductId_AddsProductToCart()
    {
        // Act
        var result = _controller.Index(1, new Uri("/", UriKind.Relative));
        
        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<CartViewModel>(viewResult.Model);
        Assert.NotNull(model.Cart);
        Assert.Single(model.Cart.Lines);
    }
    [Fact]
    public void IndexPost_InvalidProductId_RedirectsToHome()
    {
        // Act
        var result = _controller.Index(10, new Uri("/", UriKind.Relative));
    
        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Home", redirectResult.ControllerName);
    }
    [Fact]
    public void IndexPost_ManyProductsInRepo_AddsCorrectProductToCart()
    {
        // Act
        var result = _controller.Index(2, new Uri("/", UriKind.Relative));
    
        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<CartViewModel>(viewResult.Model);
        Assert.NotNull(model.Cart);
        Assert.Single(model.Cart.Lines);
        Assert.Equal(2, model.Cart.Lines[0].Product.ProductId);
    }
    [Fact]
    public void IndexPost_ZeroProductId_RedirectsToHome()
    {
        // Act
        var result = _controller.Index(0, new Uri("/", UriKind.Relative));
    
        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Home", redirectResult.ControllerName);
    }
    [Fact]
    public void IndexPost_RepositoryThrowsException_PropagatesException()
    {
        // Act
        _mockRepository.Setup(r => r.Products)
            .Throws<Exception>();
    
        // Assert
        Assert.Throws<Exception>(() => 
            _controller.Index(1, new Uri("/", UriKind.Relative)));
    }
    [Fact]
    public void IndexPost_SameProductAddedTwice_IncrementsQuantity()
    {
        // Act
        _controller.Index(2, new Uri("/", UriKind.Relative));
        var result = _controller.Index(2, new Uri("/", UriKind.Relative));
    
        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<CartViewModel>(viewResult.Model);
        Assert.NotNull(model.Cart);
        Assert.Single(model.Cart.Lines);
        Assert.Equal(2, _cart.Lines[0].Quantity);
    }
    [Fact]
    public void Remove_ValidProductId_RemovesProductFromCart()
    {
        // Arrange
        _cart.AddItem(_testProduct, 1);
        
        // Act
        var result = _controller.Remove(1, new Uri("/", UriKind.Relative));
    
        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<CartViewModel>(viewResult.Model);
        Assert.NotNull(model.Cart);
        Assert.Empty(model.Cart.Lines);
    }
    [Fact]
    public void Remove_EmptyCart_DoesNotThrow()
    {
        // Arrange
        // Act
        var exception = Record.Exception(() =>  _controller.Remove(1, new Uri("/", UriKind.Relative)));
    
        // Assert
        Assert.Null(exception);
    }
    [Fact]
    public void Remove_MultipleProducts_RemovesOnlyCorrectProduct()
    {
        // Arrange
        _cart.AddItem(_testProduct, 1);
        _cart.AddItem(_testProduct2, 1);
        
        // Act
        var result = _controller.Remove(2, new Uri("/", UriKind.Relative));
    
        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<CartViewModel>(viewResult.Model);
        Assert.NotNull(model.Cart);
        Assert.Single(model.Cart.Lines);
        Assert.Equal(1, _cart.Lines[0].Product.ProductId);
    }
    [Fact]
    public void Remove_InvalidProductId_DoesNotThrow()
    {
        // Arrange
        _cart.AddItem(_testProduct, 1);
        
        // Act
        var exception = Record.Exception(() => _controller.Remove(100, new Uri("/", UriKind.Relative)));
    
        // Assert
        Assert.Null(exception);
    }
    [Fact]
    public void Remove_AddThenRemove_CartIsEmpty()
    {
        // Arrange
        _cart.AddItem(_testProduct, 1);
        
        // Act
       _controller.Remove(1, new Uri("/", UriKind.Relative));
    
        // Assert
        Assert.Empty(_cart.Lines);
    }
    [Fact]
    public void IndexGet_EmptyCart_ReturnsCartViewModelWithEmptyLines()
    {
        // Arrange
        
        // Act
        var result = _controller.Index("/");
    
        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<CartViewModel>(viewResult.Model);
        Assert.NotNull(model.Cart);
        Assert.Empty(model.Cart.Lines);
    }
    [Fact]
    public void IndexGet_CartWithOneProduct_ReturnsCartViewModelWithProduct()
    {
        // Arrange
        _cart.AddItem(_testProduct, 1);
        
        // Act
        var result = _controller.Index("/");
    
        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<CartViewModel>(viewResult.Model);
        Assert.NotNull(model.Cart);
        Assert.Single(model.Cart.Lines);
        Assert.Equal(1, _cart.Lines[0].Product.ProductId);
    }
    [Fact]
    public void IndexGet_CartWithManyProducts_ReturnsAllProductsInViewModel()
    {
        // Arrange
        _cart.AddItem(_testProduct, 1);
        _cart.AddItem(_testProduct2, 1);
        
        // Act
        var result = _controller.Index("/");
    
        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<CartViewModel>(viewResult.Model);
        Assert.NotNull(model.Cart);
        Assert.Equal(2, model.Cart.Lines.Count);
        Assert.Equal(1, _cart.Lines[0].Product.ProductId);
        Assert.Equal(2, _cart.Lines[1].Product.ProductId);
    }
}