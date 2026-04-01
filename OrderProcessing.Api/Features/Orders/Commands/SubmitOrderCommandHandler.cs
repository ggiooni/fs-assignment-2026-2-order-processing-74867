using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderProcessing.Api.Data;
using OrderProcessing.Shared.DTOs;
using OrderProcessing.Shared.Entities;
using OrderProcessing.Shared.Enums;
using OrderProcessing.Shared.Events;
using OrderProcessing.Shared.Messaging;

namespace OrderProcessing.Api.Features.Orders.Commands;

public class SubmitOrderCommandHandler : IRequestHandler<SubmitOrderCommand, OrderDto>
{
    private readonly OrderProcessingDbContext _db;
    private readonly IRabbitMqPublisher _publisher;
    private readonly IMapper _mapper;
    private readonly ILogger<SubmitOrderCommandHandler> _logger;

    public SubmitOrderCommandHandler(
        OrderProcessingDbContext db,
        IRabbitMqPublisher publisher,
        IMapper mapper,
        ILogger<SubmitOrderCommandHandler> logger)
    {
        _db = db;
        _publisher = publisher;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<OrderDto> Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await _db.Customers.FindAsync(new object[] { request.CustomerId }, cancellationToken)
            ?? throw new InvalidOperationException($"Customer {request.CustomerId} not found");

        var productIds = request.Items.Select(i => i.ProductId).ToList();
        var products = await _db.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, cancellationToken);

        var order = new Order
        {
            CustomerId = request.CustomerId,
            Status = OrderStatus.Submitted,
            OrderDate = DateTime.UtcNow
        };

        foreach (var item in request.Items)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
                throw new InvalidOperationException($"Product {item.ProductId} not found");

            order.Items.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        order.TotalAmount = order.Items.Sum(i => i.Quantity * i.UnitPrice);

        _db.Orders.Add(order);
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Order {OrderId} created for customer {CustomerId} with total {TotalAmount}",
            order.Id, order.CustomerId, order.TotalAmount);

        var orderEvent = new OrderSubmittedEvent
        {
            OrderId = order.Id,
            CustomerId = order.CustomerId,
            TotalAmount = order.TotalAmount,
            CorrelationId = order.CorrelationId,
            Items = order.Items.Select(i => new OrderItemEvent
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        await _publisher.PublishAsync(QueueNames.OrderSubmitted, orderEvent);

        _logger.LogInformation("OrderSubmittedEvent published for order {OrderId}", order.Id);

        // Reload with includes for mapping
        var savedOrder = await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .FirstAsync(o => o.Id == order.Id, cancellationToken);

        return _mapper.Map<OrderDto>(savedOrder);
    }
}
