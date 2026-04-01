using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using OrderProcessing.Api.Features.Orders.Commands;
using OrderProcessing.Shared.DTOs;
using OrderProcessing.Shared.Entities;
using OrderProcessing.Shared.Enums;
using OrderProcessing.Shared.Messaging;
using OrderProcessing.Tests.Helpers;

namespace OrderProcessing.Tests.Commands;

public class SubmitOrderCommandHandlerTests
{
    private readonly Mock<IRabbitMqPublisher> _publisherMock;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<SubmitOrderCommandHandler>> _loggerMock;

    public SubmitOrderCommandHandlerTests()
    {
        _publisherMock = new Mock<IRabbitMqPublisher>();
        _mapper = TestMapperFactory.Create();
        _loggerMock = new Mock<ILogger<SubmitOrderCommandHandler>>();
    }

    [Fact]
    public async Task Handle_ValidOrder_CreatesOrderAndPublishesEvent()
    {
        // Arrange
        var db = TestDbContextFactory.Create();
        db.Customers.Add(new Customer { Id = 10, Name = "Test", Email = "test@test.com" });
        db.Products.Add(new Product { Id = 10, Name = "Widget", Price = 25.00m, Category = "Test", StockQuantity = 10 });
        await db.SaveChangesAsync();

        var handler = new SubmitOrderCommandHandler(db, _publisherMock.Object, _mapper, _loggerMock.Object);
        var command = new SubmitOrderCommand(10, new List<CartItemRequest>
        {
            new() { ProductId = 10, Quantity = 2 }
        });

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(50.00m, result.TotalAmount);
        Assert.Equal("Submitted", result.Status);
        Assert.Single(result.Items);

        _publisherMock.Verify(p => p.PublishAsync(
            QueueNames.OrderSubmitted,
            It.IsAny<OrderProcessing.Shared.Events.OrderSubmittedEvent>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidCustomer_ThrowsException()
    {
        var db = TestDbContextFactory.Create();
        var handler = new SubmitOrderCommandHandler(db, _publisherMock.Object, _mapper, _loggerMock.Object);
        var command = new SubmitOrderCommand(999, new List<CartItemRequest>
        {
            new() { ProductId = 1, Quantity = 1 }
        });

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidProduct_ThrowsException()
    {
        var db = TestDbContextFactory.Create();
        db.Customers.Add(new Customer { Id = 20, Name = "Test", Email = "test@test.com" });
        await db.SaveChangesAsync();

        var handler = new SubmitOrderCommandHandler(db, _publisherMock.Object, _mapper, _loggerMock.Object);
        var command = new SubmitOrderCommand(20, new List<CartItemRequest>
        {
            new() { ProductId = 999, Quantity = 1 }
        });

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(command, CancellationToken.None));
    }
}
