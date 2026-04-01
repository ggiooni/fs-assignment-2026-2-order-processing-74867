using MediatR;
using OrderProcessing.Shared.DTOs;

namespace OrderProcessing.Api.Features.Orders.Queries;

public record GetOrdersQuery(string? Status = null) : IRequest<List<OrderDto>>;
