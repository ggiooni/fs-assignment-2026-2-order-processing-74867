namespace OrderProcessing.Shared.Events;

public class InventoryConfirmedEvent : IntegrationEvent
{
    public int OrderId { get; set; }
}

public class InventoryFailedEvent : IntegrationEvent
{
    public int OrderId { get; set; }
    public string Reason { get; set; } = string.Empty;
}
