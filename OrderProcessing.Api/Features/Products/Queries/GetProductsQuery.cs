using MediatR;
using OrderProcessing.Shared.DTOs;

namespace OrderProcessing.Api.Features.Products.Queries;

public record GetProductsQuery(string? Category = null) : IRequest<List<ProductDto>>;
