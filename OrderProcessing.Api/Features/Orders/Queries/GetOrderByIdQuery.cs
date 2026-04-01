using MediatR;
using OrderProcessing.Shared.DTOs;

namespace OrderProcessing.Api.Features.Orders.Queries;

public record GetOrderByIdQuery(int Id) : IRequest<OrderDto?>;
