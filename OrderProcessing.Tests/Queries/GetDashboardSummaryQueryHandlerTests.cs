using OrderProcessing.Api.Features.Orders.Queries;
using OrderProcessing.Shared.Entities;
using OrderProcessing.Shared.Enums;
using OrderProcessing.Tests.Helpers;

namespace OrderProcessing.Tests.Queries;

public class GetDashboardSummaryQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsSummaryWithCorrectCounts()
    {
        var db = TestDbContextFactory.Create();
        var customer = new Customer { Id = 50, Name = "Test", Email = "t@t.com" };
        db.Customers.Add(customer);
        db.Orders.Add(new Order { CustomerId = 50, Status = OrderStatus.Completed, TotalAmount = 100 });
        db.Orders.Add(new Order { CustomerId = 50, Status = OrderStatus.Completed, TotalAmount = 200 });
        db.Orders.Add(new Order { CustomerId = 50, Status = OrderStatus.Failed, TotalAmount = 50 });
        db.Orders.Add(new Order { CustomerId = 50, Status = OrderStatus.PaymentPending, TotalAmount = 75 });
        await db.SaveChangesAsync();

        var handler = new GetDashboardSummaryQueryHandler(db);
        var result = await handler.Handle(new GetDashboardSummaryQuery(), CancellationToken.None);

        Assert.Equal(4, result.TotalOrders);
        Assert.Equal(2, result.CompletedOrders);
        Assert.Equal(1, result.FailedOrders);
        Assert.Equal(1, result.PendingOrders);
        Assert.Equal(300m, result.TotalRevenue);
    }

    [Fact]
    public async Task Handle_EmptyDb_ReturnsZeroCounts()
    {
        var db = TestDbContextFactory.Create();
        var handler = new GetDashboardSummaryQueryHandler(db);
        var result = await handler.Handle(new GetDashboardSummaryQuery(), CancellationToken.None);

        Assert.Equal(0, result.TotalOrders);
        Assert.Equal(0m, result.TotalRevenue);
    }
}
