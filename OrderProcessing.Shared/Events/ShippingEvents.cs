namespace OrderProcessing.Shared.Events;

public class ShippingCreatedEvent : IntegrationEvent
{
    public int OrderId { get; set; }
    public string TrackingNumber { get; set; } = string.Empty;
    public string Carrier { get; set; } = string.Empty;
    public DateTime EstimatedDelivery { get; set; }
}

public class ShippingFailedEvent : IntegrationEvent
{
    public int OrderId { get; set; }
    public string Reason { get; set; } = string.Empty;
}
