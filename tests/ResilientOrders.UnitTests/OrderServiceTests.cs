using FluentAssertions;
using Moq;
using ResilientOrders.Api.Models;
using ResilientOrders.Api.Services;
using Xunit;

namespace ResilientOrders.UnitTests;

public class OrderServiceTests
{
    [Fact(DisplayName = "CalculateTotal: aplica desconto corretamente sobre o subtotal")]
    public void CalculateTotal_WithValidDiscount_ReturnsExpectedValue()
    {
        // Arrange
        var mockRepo = new Mock<IOrderRepository>();
        var service = new OrderService(mockRepo.Object);

        // Act
        var result = service.CalculateTotal(100m, 0.10m);

        // Assert
        result.Should().Be(90m);
    }

    [Fact(DisplayName = "CalculateTotal: desconto negativo lança ArgumentException")]
    public void CalculateTotal_WithNegativeDiscount_ThrowsArgumentException()
    {
        // Arrange
        var mockRepo = new Mock<IOrderRepository>();
        var service = new OrderService(mockRepo.Object);

        // Act
        Action action = () => service.CalculateTotal(100m, -0.5m);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "PlaceOrder: pedido sem itens lança InvalidOperationException")]
    public void PlaceOrder_WithEmptyItems_ThrowsInvalidOperationException()
    {
        // Arrange
        var mockRepo = new Mock<IOrderRepository>();
        var service = new OrderService(mockRepo.Object);

        var request = new CreateOrderRequest(
            "Maria",
            new List<OrderItem>(),
            0m
        );

        // Act
        Action action = () => service.PlaceOrder(request);

        // Assert
        action.Should().Throw<InvalidOperationException>();

        mockRepo.Verify(r => r.Save(It.IsAny<Order>()), Times.Never);
    }
}