using Microsoft.AspNetCore.Mvc;
using Moq;
using PackagingWholesale.Controllers;
using PackagingWholesale.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Arrange - define imitation of the repository
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            // Arrange - create empty cart
            Cart cart = new Cart();

            // Arrange - define order
            Order order = new Order();

            // Arrange - creating a copy of the controller
            OrderController target = new OrderController(mock.Object, cart);

            // Act
            ViewResult result = target.Checkout(order) as ViewResult;

            // Assert - checking if order has been placed in the repository
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            // Assert - checking if method return default view
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            // Assert - checking if pass right model to the view
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Arrange - define imitation of the repository
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            // Arrange - define cart with the product
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            // Arrange - creating a copy of the controller
            OrderController target = new OrderController(mock.Object, cart);
            // Arrange - add error to the model
            target.ModelState.AddModelError("error", "error");

            // Act - try end of the order
            ViewResult result = target.Checkout(new Order()) as ViewResult;

            // Assert - checking if order is not been placed in the repository
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            // Assert - checking if method return default view
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            // Assert - checking if we pass incorrect model to the view
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            // Arrange - define imitation of the repository
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            // Arrange - define cart with the product
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            // Arrange - creating a copy of the  Controller
            OrderController target = new OrderController(mock.Object, cart);

            // Act - try end of the order
            RedirectToActionResult result = target.Checkout(new Order()) as RedirectToActionResult;

            // Assert - checking if order is not been placed in the repository
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            // Assert - checking if method redirects to the action method Completed()
            Assert.Equal("Completed", result.ActionName);
        }
    }
}
