using MediatR;
using OrderProcessing.Shared.DTOs;

namespace OrderProcessing.Api.Features.Orders.Queries;

public record GetCustomerOrdersQuery(int CustomerId) : IRequest<List<OrderDto>>;
