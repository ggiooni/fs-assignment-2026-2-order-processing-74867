using MediatR;
using OrderProcessing.Shared.DTOs;

namespace OrderProcessing.Api.Features.Orders.Commands;

public record SubmitOrderCommand(int CustomerId, List<CartItemRequest> Items) : IRequest<OrderDto>;
