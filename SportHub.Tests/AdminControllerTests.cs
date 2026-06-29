using Microsoft.AspNetCore.Mvc;
using Moq;
using SportHub.Controllers;
using SportHub.Migrations;
using SportHub.Models.Domain;
using SportHub.Models.Repository;

namespace SportHub.Tests;

public class AdminControllerTests
{
    private static (AdminController controller, Mock<IHubRepository> hubRepo,Mock<IOrderRepository> orderRepo) BuildSut
        (IEnumerable<Product>? products = null, IEnumerable<Order>? orders = null)
    {
        var hubRepo = new Mock<IHubRepository>();
        var orderRepo = new Mock<IOrderRepository>();
        
        hubRepo.Setup(r => r.Products)
            .Returns((products ?? []).AsQueryable());
        orderRepo.Setup(r => r.Orders)
            .Returns((orders ?? []).AsQueryable());
        
        var controller = new AdminController(hubRepo.Object, orderRepo.Object);
        return (controller, hubRepo, orderRepo);
    }

    [Fact]
    public void Orders_WhenNoOrders_ReturnsViewWithEmptyModel()
    {
        // Arrange
        var (controller, _, _) = BuildSut(orders: []);
        
        // Act
        var result = controller.Orders();
        
        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Empty((IEnumerable<Order>)viewResult.Model!);
    }
    
    [Fact]
    public void Products_WhenNoProducts_ReturnsViewWithEmptyModel()
    {
        // Arrange
        var (controller, _, _) = BuildSut(products: []);
        
        // Act
        var result = controller.Products();
        
        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Empty((IEnumerable<Product>)viewResult.Model!);
    }
    
    [Fact]
    public void Details_ExistingProductId_ReturnsViewWithCorrectProduct()
    {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Nike Shoes", Price = 99.99m };
        var (controller, _, _) = BuildSut(products: [product]);
        
        // Act
        var result = controller.Details(1);
        
        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(product, viewResult.Model);
    }
    
    [Fact]
    public void Edit_Get_ExistingProductId_ReturnsViewWithCorrectProduct()
    {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Nike Shoes", Price = 99.99m };
        var (controller, _, _) = BuildSut(products: [product]);
        
        // Act
        var result = controller.Edit(1);
        
        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(product, viewResult.Model!);
    }
    
    [Fact]
    public void Products_WithManyProducts_ReturnsAllProductsInView()
    {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Nike Shoes", Price = 99.99m };
        var product2 = new Product { ProductId = 2, Name = "Nike Bag", Price = 35.99m };
        var (controller, _, _) = BuildSut(products: [product, product2]);

        // Act
        var result = controller.Products();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(2, ((IEnumerable<Product>)viewResult.Model!).Count());
    }

    [Fact]
    public void Orders_WithManyOrders_ReturnsAllOrdersInView()
    {
        // Arrange
        List<Order> orders = Enumerable.Range(1, 3)
            .Select(i => new Order { OrderId = i })
            .ToList();
        var (controller, _, _) = BuildSut(orders: orders);

        // Act
        var result = controller.Orders();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(3, ((IEnumerable<Order>)viewResult.Model!).Count());
    }
    
    [Fact]
    public void Details_NonExistentProductId_ReturnsViewWithNullModel()
    {
        // Arrange
        var (controller, _, _) = BuildSut(products: []);

        // Act
        var result = controller.Details(int.MaxValue);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.Model);
    }

    [Fact]
    public void Delete_Get_NonExistentProductId_ReturnsViewWithNullModel()
    {
        // Arrange
        var (controller, _, _) = BuildSut(products: []);

        // Act
        var result = controller.Delete(999);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.Model);
    }
    
    [Fact]
    public void Edit_Post_ValidProduct_CallsUpdateProductOnce()
    {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Nike Shoes", Price = 99.99m };
        var (controller, hubRepo, _) = BuildSut();

        // Act
        controller.Edit(product);

        // Assert
        hubRepo.Verify(r => r.UpdateProduct(product), Times.Once);
    }

    [Fact]
    public void Create_Post_ValidProduct_CallsCreateProductOnce()
    {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Nike Shoes", Price = 99.99m };
        var (controller, hubRepo, _) = BuildSut();

        // Act
        controller.Create(product);

        // Assert
        hubRepo.Verify(r => r.CreateProduct(product), Times.Once);
    }

    [Fact]
    public void DeleteProduct_ExistingProduct_CallsDeleteProductOnce()
    {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Nike Shoes", Price = 99.99m };
        var (controller, hubRepo, _) = BuildSut(products: [product]);

        // Act
        controller.DeleteProduct(1);

        // Assert
        hubRepo.Verify(r => r.DeleteProduct(product), Times.Once);
    }

    [Fact]
    public void DeleteProduct_NonExistentId_NeverCallsDeleteProduct()
    {
        // Arrange
        var (controller, hubRepo, _) = BuildSut(products: []);

        // Act
        controller.DeleteProduct(999);

        // Assert
        hubRepo.Verify(r => r.DeleteProduct(It.IsAny<Product>()), Times.Never);
    }
    
    [Fact]
    public void Edit_Post_InvalidModelState_ReturnsViewWithSameProduct()
    {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Nike Shoes", Price = 99.99m };
        var (controller, hubRepo, _) = BuildSut(products: [product]);
        controller.ModelState.AddModelError("Name", "Required");

        // Act
        var result = controller.Edit(product);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(product, viewResult.Model);
        hubRepo.Verify(r => r.UpdateProduct(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public void Create_Post_InvalidModelState_ReturnsViewWithSameProduct()
    {
        // Arrange
        var product = new Product();
        var (controller, hubRepo, _) = BuildSut();
        controller.ModelState.AddModelError("Name", "Required");

        // Act
        var result = controller.Create(product);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(product, viewResult.Model);
        hubRepo.Verify(r => r.CreateProduct(It.IsAny<Product>()), Times.Never);
    }
    
    [Fact]
    public void MarkShipped_ExistingOrder_SetsShippedTrueAndSavesOrder()
    {
        // Arrange
        var order = new Order { OrderId = 10, Shipped = false };
        var (controller, _, orderRepo) = BuildSut(orders: [order]);

        // Act
        controller.MarkShipped(10);

        // Assert
        Assert.True(order.Shipped);
        orderRepo.Verify(r => r.SaveOrder(order), Times.Once);
    }

    [Fact]
    public void MarkShipped_NonExistentOrder_RedirectsToOrdersWithoutSaving()
    {
        // Arrange
        var (controller, _, orderRepo) = BuildSut(orders: []);

        // Act
        var result = controller.MarkShipped(999);

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Orders", redirect.ActionName);
        orderRepo.Verify(r => r.SaveOrder(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public void Reset_ExistingOrder_SetsShippedFalseAndSavesOrder()
    {
        // Arrange
        var order = new Order { OrderId = 5, Shipped = true };
        var (controller, _, orderRepo) = BuildSut(orders: [order]);

        // Act
        controller.Reset(5);

        // Assert
        Assert.False(order.Shipped);
        orderRepo.Verify(r => r.SaveOrder(order), Times.Once);
    }

    [Fact]
    public void Reset_NonExistentOrder_RedirectsToOrdersWithoutSaving()
    {
        // Arrange
        var (controller, _, orderRepo) = BuildSut(orders: []);

        // Act
        var result = controller.Reset(404);

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Orders", redirect.ActionName);
        orderRepo.Verify(r => r.SaveOrder(It.IsAny<Order>()), Times.Never);
    }
    
    [Fact]
    public void Constructor_NullHubRepository_ThrowsArgumentNullException()
    {
        // Arrange
        var orderRepo = new Mock<IOrderRepository>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new AdminController(null!, orderRepo.Object));
    }

    [Fact]
    public void Constructor_NullOrderRepository_ThrowsArgumentNullException()
    {
        // Arrange
        var hubRepo = new Mock<IHubRepository>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new AdminController(hubRepo.Object, null!));
    }
}