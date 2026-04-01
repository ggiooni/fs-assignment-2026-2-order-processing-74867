using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderProcessing.Api.Data;
using OrderProcessing.Shared.DTOs;

namespace OrderProcessing.Api.Features.Products.Queries;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly OrderProcessingDbContext _db;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(OrderProcessingDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Products.AsQueryable();

        if (!string.IsNullOrEmpty(request.Category))
            query = query.Where(p => p.Category == request.Category);

        var products = await query.OrderBy(p => p.Name).ToListAsync(cancellationToken);
        return _mapper.Map<List<ProductDto>>(products);
    }
}
