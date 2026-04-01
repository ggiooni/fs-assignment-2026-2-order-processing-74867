using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderProcessing.Api.Data;
using OrderProcessing.Shared.DTOs;
using OrderProcessing.Shared.Enums;

namespace OrderProcessing.Api.Features.Orders.Queries;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderDto>>
{
    private readonly OrderProcessingDbContext _db;
    private readonly IMapper _mapper;

    public GetOrdersQueryHandler(OrderProcessingDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .Include(o => o.PaymentRecord)
            .Include(o => o.ShipmentRecord)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<OrderStatus>(request.Status, true, out var status))
        {
            query = query.Where(o => o.Status == status);
        }

        var orders = await query.OrderByDescending(o => o.OrderDate).ToListAsync(cancellationToken);
        return _mapper.Map<List<OrderDto>>(orders);
    }
}
