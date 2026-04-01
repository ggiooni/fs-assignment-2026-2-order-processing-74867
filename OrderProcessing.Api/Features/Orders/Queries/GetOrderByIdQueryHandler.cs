using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderProcessing.Api.Data;
using OrderProcessing.Shared.DTOs;

namespace OrderProcessing.Api.Features.Orders.Queries;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly OrderProcessingDbContext _db;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(OrderProcessingDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .Include(o => o.PaymentRecord)
            .Include(o => o.ShipmentRecord)
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

        return order is null ? null : _mapper.Map<OrderDto>(order);
    }
}
