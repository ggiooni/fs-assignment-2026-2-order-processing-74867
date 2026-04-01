using AutoMapper;
using OrderProcessing.Api.Features.Orders.Queries;
using OrderProcessing.Shared.Entities;
using OrderProcessing.Shared.Enums;
using OrderProcessing.Tests.Helpers;

namespace OrderProcessing.Tests.Queries;

public class GetOrderByIdQueryHandlerTests
{
    private readonly IMapper _mapper = TestMapperFactory.Create();

    [Fact]
    public async Task Handle_ExistingOrder_ReturnsOrderDto()
    {
        var db = TestDbContextFactory.Create();
        var customer = new Customer { Id = 40, Name = "Charlie", Email = "c@c.com" };
        db.Customers.Add(customer);
        var order = new Order { CustomerId = 40, Customer = customer, Status = OrderStatus.PaymentApproved, TotalAmount = 200 };
        db.Orders.Add(order);
        await db.SaveChangesAsync();

        var handler = new GetOrderByIdQueryHandler(db, _mapper);
        var result = await handler.Handle(new GetOrderByIdQuery(order.Id), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("PaymentApproved", result!.Status);
        Assert.Equal(200m, result.TotalAmount);
    }

    [Fact]
    public async Task Handle_NonExistingOrder_ReturnsNull()
    {
        var db = TestDbContextFactory.Create();
        var handler = new GetOrderByIdQueryHandler(db, _mapper);
        var result = await handler.Handle(new GetOrderByIdQuery(999), CancellationToken.None);

        Assert.Null(result);
    }
}
