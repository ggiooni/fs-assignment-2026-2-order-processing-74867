using AutoMapper;
using OrderProcessing.Api.Features.Orders.Queries;
using OrderProcessing.Shared.Entities;
using OrderProcessing.Shared.Enums;
using OrderProcessing.Tests.Helpers;

namespace OrderProcessing.Tests.Queries;

public class GetOrdersQueryHandlerTests
{
    private readonly IMapper _mapper = TestMapperFactory.Create();

    [Fact]
    public async Task Handle_ReturnsAllOrders()
    {
        var db = TestDbContextFactory.Create();
        var customer = new Customer { Id = 30, Name = "Alice", Email = "a@b.com" };
        db.Customers.Add(customer);
        db.Orders.Add(new Order { CustomerId = 30, Customer = customer, Status = OrderStatus.Completed, TotalAmount = 50 });
        db.Orders.Add(new Order { CustomerId = 30, Customer = customer, Status = OrderStatus.Submitted, TotalAmount = 75 });
        await db.SaveChangesAsync();

        var handler = new GetOrdersQueryHandler(db, _mapper);
        var result = await handler.Handle(new GetOrdersQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Handle_FiltersByStatus()
    {
        var db = TestDbContextFactory.Create();
        var customer = new Customer { Id = 31, Name = "Bob", Email = "b@b.com" };
        db.Customers.Add(customer);
        db.Orders.Add(new Order { CustomerId = 31, Customer = customer, Status = OrderStatus.Completed, TotalAmount = 50 });
        db.Orders.Add(new Order { CustomerId = 31, Customer = customer, Status = OrderStatus.Failed, TotalAmount = 75 });
        await db.SaveChangesAsync();

        var handler = new GetOrdersQueryHandler(db, _mapper);
        var result = await handler.Handle(new GetOrdersQuery("Completed"), CancellationToken.None);

        Assert.Single(result);
        Assert.Equal("Completed", result[0].Status);
    }

    [Fact]
    public async Task Handle_EmptyDb_ReturnsEmptyList()
    {
        var db = TestDbContextFactory.Create();
        var handler = new GetOrdersQueryHandler(db, _mapper);
        var result = await handler.Handle(new GetOrdersQuery(), CancellationToken.None);

        Assert.Empty(result);
    }
}
