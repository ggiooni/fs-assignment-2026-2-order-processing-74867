using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderProcessing.Api.Data;
using OrderProcessing.Shared.DTOs;
using OrderProcessing.Shared.Enums;

namespace OrderProcessing.Api.Features.Orders.Queries;

public class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
{
    private readonly OrderProcessingDbContext _db;

    public GetDashboardSummaryQueryHandler(OrderProcessingDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        var orders = await _db.Orders.ToListAsync(cancellationToken);

        return new DashboardSummaryDto
        {
            TotalOrders = orders.Count,
            PendingOrders = orders.Count(o => o.Status != OrderStatus.Completed && o.Status != OrderStatus.Failed),
            CompletedOrders = orders.Count(o => o.Status == OrderStatus.Completed),
            FailedOrders = orders.Count(o => o.Status == OrderStatus.Failed ||
                                              o.Status == OrderStatus.InventoryFailed ||
                                              o.Status == OrderStatus.PaymentFailed),
            TotalRevenue = orders.Where(o => o.Status == OrderStatus.Completed).Sum(o => o.TotalAmount)
        };
    }
}
