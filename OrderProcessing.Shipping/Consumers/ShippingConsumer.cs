using Microsoft.Extensions.Options;
using OrderProcessing.Api.Data;
using OrderProcessing.Shared.Entities;
using OrderProcessing.Shared.Enums;
using OrderProcessing.Shared.Events;
using OrderProcessing.Shared.Messaging;

namespace OrderProcessing.Shipping.Consumers;

public class ShippingConsumer : RabbitMqConsumer<PaymentApprovedEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IRabbitMqPublisher _publisher;
    private readonly ILogger<ShippingConsumer> _logger;

    public ShippingConsumer(
        IOptions<RabbitMqSettings> settings,
        ILogger<ShippingConsumer> logger,
        IServiceScopeFactory scopeFactory,
        IRabbitMqPublisher publisher)
        : base(settings, logger, QueueNames.PaymentApproved)
    {
        _scopeFactory = scopeFactory;
        _publisher = publisher;
        _logger = logger;
    }

    protected override async Task HandleMessageAsync(PaymentApprovedEvent message)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<OrderProcessingDbContext>();

        var order = await db.Orders.FindAsync(message.OrderId);
        if (order is null) return;

        order.Status = OrderStatus.ShippingPending;
        await db.SaveChangesAsync();

        _logger.LogInformation("Creating shipment for order {OrderId}, CorrelationId: {CorrelationId}",
            message.OrderId, message.CorrelationId);

        // Simulate shipping processing
        await Task.Delay(500);

        var trackingNumber = $"TRACK-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        var estimatedDelivery = DateTime.UtcNow.AddDays(5);

        var shipment = new ShipmentRecord
        {
            OrderId = message.OrderId,
            TrackingNumber = trackingNumber,
            Carrier = "Express Shipping",
            Status = ShipmentStatus.Shipped,
            ShippedAt = DateTime.UtcNow,
            EstimatedDelivery = estimatedDelivery
        };

        db.ShipmentRecords.Add(shipment);
        order.Status = OrderStatus.Completed;
        await db.SaveChangesAsync();

        await _publisher.PublishAsync(QueueNames.ShippingCreated, new ShippingCreatedEvent
        {
            OrderId = message.OrderId,
            TrackingNumber = trackingNumber,
            Carrier = "Express Shipping",
            EstimatedDelivery = estimatedDelivery,
            CorrelationId = message.CorrelationId
        });

        _logger.LogInformation("Shipment created for order {OrderId}, Tracking: {TrackingNumber}",
            message.OrderId, trackingNumber);
    }
}
