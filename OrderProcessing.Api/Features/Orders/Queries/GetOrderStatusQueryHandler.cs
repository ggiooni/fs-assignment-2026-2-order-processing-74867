using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderProcessing.Api.Data;
using OrderProcessing.Shared.DTOs;

namespace OrderProcessing.Api.Features.Orders.Queries;

public class GetOrderStatusQueryHandler : IRequestHandler<GetOrderStatusQuery, OrderStatusDto?>
{
    private readonly OrderProcessingDbContext _db;
    private readonly IMapper _mapper;

    public GetOrderStatusQueryHandler(OrderProcessingDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<OrderStatusDto?> Handle(GetOrderStatusQuery request, CancellationToken cancellationToken)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        return order is null ? null : _mapper.Map<OrderStatusDto>(order);
    }
}
