using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using PackagingWholesale.Controllers;
using PackagingWholesale.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void Index_Contains_All_Products()
        {
            // Arrange - creat imitation of repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name ="P1" },
                new Product { ProductID = 2 , Name ="P2"},
                new Product {ProductID =3 , Name = "P3"}
            }.AsQueryable<Product>());

            // Arrange - define controller
            AdminController target = new AdminController(mock.Object);

            // Act
            Product[] result = GetViewModel<IEnumerable<Product>>(target.Index())?.ToArray();

            // Assert
            Assert.Equal(3, result.Length);
            Assert.Equal("P1", result[0].Name);
            Assert.Equal("P2", result[1].Name);
            Assert.Equal("P3", result[2].Name);
        }
        private T GetViewModel<T>(IActionResult result) where T: class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }

        [Fact]
        public void Can_Edit_Product()
        {
            // Arrange - create imitation of the repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name ="P3"}
            }.AsQueryable<Product>());

            // Arrange - define controller
            AdminController target = new AdminController(mock.Object);

            // Act
            Product p1 = GetViewModel<Product>(target.Edit(1));
            Product p2 = GetViewModel<Product>(target.Edit(2));
            Product p3 = GetViewModel<Product>(target.Edit(3));

            // Assert
            Assert.Equal(1, p1.ProductID);
            Assert.Equal(2, p2.ProductID);
            Assert.Equal(3, p3.ProductID);
        }

        [Fact]
        public void Cannot_Edit_Nonexistent_Product()
        {
            // Arrange - creat imitation of repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name ="P3"}
            }.AsQueryable<Product>());

            // Arrange - define controller
            AdminController target = new AdminController(mock.Object);

            // Act
            Product result = GetViewModel<Product>(target.Edit(4));

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            // Arrange - create imitation of repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            // Arrange - create imitation of the container TempData
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            // Arrange - create new controller
            AdminController target = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };
            // Arrange - create new product
            Product product = new Product { Name = "Test" };

            // Act
            IActionResult result = target.Edit(product);

            // Assert - trying to save the product
            mock.Verify(m => m.SaveProduct(product));
            // Assert -  checking the return type of a method
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            // Arrange - create imitation of repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            // Arrange - create new controller
            AdminController target = new AdminController(mock.Object);
            // Arrange - create new product
            Product product = new Product { Name = "Test" };
            // Arrange - add error to the state of model
            target.ModelState.AddModelError("error", "error");

            // Act
            IActionResult result = target.Edit(product);

            // Assert - checked whether the repository was called 
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            // Assert -  checking the return type of a method
            Assert.IsType<ViewResult>(result);

        }

        [Fact]
        public void Can_Delete_Valid_Products()
        {
            // Arrange - create new product
            Product product = new Product { ProductID = 2, Name="Test" };
            // Arrange - create imitation of repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                product,
                new Product {ProductID = 3, Name ="P3"}
            }.AsQueryable<Product>());
            // Arrange - create new controller
            AdminController target = new AdminController(mock.Object);

            // Act
            target.Delete(product.ProductID);

            // Assert 
            mock.Verify(m => m.DeleteProduct(product.ProductID));

        }
    }
}
