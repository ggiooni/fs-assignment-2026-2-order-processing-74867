using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderProcessing.Api.Data;
using OrderProcessing.Shared.DTOs;

namespace OrderProcessing.Api.Features.Orders.Queries;

public class GetCustomerOrdersQueryHandler : IRequestHandler<GetCustomerOrdersQuery, List<OrderDto>>
{
    private readonly OrderProcessingDbContext _db;
    private readonly IMapper _mapper;

    public GetCustomerOrdersQueryHandler(OrderProcessingDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<OrderDto>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .Include(o => o.PaymentRecord)
            .Include(o => o.ShipmentRecord)
            .Where(o => o.CustomerId == request.CustomerId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<OrderDto>>(orders);
    }
}
