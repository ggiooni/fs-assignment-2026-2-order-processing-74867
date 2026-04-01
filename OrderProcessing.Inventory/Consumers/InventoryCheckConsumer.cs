using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrderProcessing.Api.Data;
using OrderProcessing.Shared.Enums;
using OrderProcessing.Shared.Events;
using OrderProcessing.Shared.Messaging;

namespace OrderProcessing.Inventory.Consumers;

public class InventoryCheckConsumer : RabbitMqConsumer<OrderSubmittedEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IRabbitMqPublisher _publisher;
    private readonly ILogger<InventoryCheckConsumer> _logger;

    public InventoryCheckConsumer(
        IOptions<RabbitMqSettings> settings,
        ILogger<InventoryCheckConsumer> logger,
        IServiceScopeFactory scopeFactory,
        IRabbitMqPublisher publisher)
        : base(settings, logger, QueueNames.OrderSubmitted)
    {
        _scopeFactory = scopeFactory;
        _publisher = publisher;
        _logger = logger;
    }

    protected override async Task HandleMessageAsync(OrderSubmittedEvent message)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<OrderProcessingDbContext>();

        var order = await db.Orders.FindAsync(message.OrderId);
        if (order is null) return;

        order.Status = OrderStatus.InventoryPending;
        await db.SaveChangesAsync();

        _logger.LogInformation("Checking inventory for order {OrderId}, CorrelationId: {CorrelationId}",
            message.OrderId, message.CorrelationId);

        foreach (var item in message.Items)
        {
            var inventory = await db.InventoryRecords
                .FirstOrDefaultAsync(i => i.ProductId == item.ProductId);

            if (inventory is null || inventory.QuantityOnHand - inventory.ReservedQuantity < item.Quantity)
            {
                order.Status = OrderStatus.InventoryFailed;
                order.FailureReason = $"Insufficient stock for product {item.ProductId}";
                await db.SaveChangesAsync();

                await _publisher.PublishAsync(QueueNames.InventoryFailed, new InventoryFailedEvent
                {
                    OrderId = message.OrderId,
                    Reason = order.FailureReason,
                    CorrelationId = message.CorrelationId
                });

                _logger.LogWarning("Inventory check failed for order {OrderId}: {Reason}",
                    message.OrderId, order.FailureReason);
                return;
            }

            inventory.ReservedQuantity += item.Quantity;
            inventory.LastUpdated = DateTime.UtcNow;
        }

        order.Status = OrderStatus.InventoryConfirmed;
        await db.SaveChangesAsync();

        await _publisher.PublishAsync(QueueNames.InventoryConfirmed, new InventoryConfirmedEvent
        {
            OrderId = message.OrderId,
            CorrelationId = message.CorrelationId
        });

        _logger.LogInformation("Inventory confirmed for order {OrderId}", message.OrderId);
    }
}
