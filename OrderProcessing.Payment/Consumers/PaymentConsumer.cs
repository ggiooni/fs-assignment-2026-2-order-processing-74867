using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrderProcessing.Api.Data;
using OrderProcessing.Shared.Entities;
using OrderProcessing.Shared.Enums;
using OrderProcessing.Shared.Events;
using OrderProcessing.Shared.Messaging;

namespace OrderProcessing.Payment.Consumers;

public class PaymentConsumer : RabbitMqConsumer<InventoryConfirmedEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IRabbitMqPublisher _publisher;
    private readonly ILogger<PaymentConsumer> _logger;
    private static readonly Random _random = new();

    public PaymentConsumer(
        IOptions<RabbitMqSettings> settings,
        ILogger<PaymentConsumer> logger,
        IServiceScopeFactory scopeFactory,
        IRabbitMqPublisher publisher)
        : base(settings, logger, QueueNames.InventoryConfirmed)
    {
        _scopeFactory = scopeFactory;
        _publisher = publisher;
        _logger = logger;
    }

    protected override async Task HandleMessageAsync(InventoryConfirmedEvent message)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<OrderProcessingDbContext>();

        var order = await db.Orders.FindAsync(message.OrderId);
        if (order is null) return;

        order.Status = OrderStatus.PaymentPending;
        await db.SaveChangesAsync();

        _logger.LogInformation("Processing payment for order {OrderId}, Amount: {Amount}, CorrelationId: {CorrelationId}",
            message.OrderId, order.TotalAmount, message.CorrelationId);

        // Simulate processing delay
        await Task.Delay(1000);

        // 90% success rate
        var isApproved = _random.Next(100) < 90;

        var paymentRecord = new PaymentRecord
        {
            OrderId = message.OrderId,
            Amount = order.TotalAmount,
            Status = isApproved ? PaymentStatus.Approved : PaymentStatus.Failed,
            TransactionId = isApproved ? $"TXN-{Guid.NewGuid():N}"[..16].ToUpper() : null,
            ProcessedAt = DateTime.UtcNow
        };

        db.PaymentRecords.Add(paymentRecord);

        if (isApproved)
        {
            order.Status = OrderStatus.PaymentApproved;
            await db.SaveChangesAsync();

            await _publisher.PublishAsync(QueueNames.PaymentApproved, new PaymentApprovedEvent
            {
                OrderId = message.OrderId,
                TransactionId = paymentRecord.TransactionId!,
                Amount = order.TotalAmount,
                CorrelationId = message.CorrelationId
            });

            _logger.LogInformation("Payment approved for order {OrderId}, Transaction: {TransactionId}",
                message.OrderId, paymentRecord.TransactionId);
        }
        else
        {
            order.Status = OrderStatus.PaymentFailed;
            order.FailureReason = "Payment declined by processor";
            await db.SaveChangesAsync();

            await _publisher.PublishAsync(QueueNames.PaymentFailed, new PaymentFailedEvent
            {
                OrderId = message.OrderId,
                Reason = "Payment declined by processor",
                CorrelationId = message.CorrelationId
            });

            _logger.LogWarning("Payment failed for order {OrderId}", message.OrderId);
        }
    }
}
