namespace OrderProcessing.Shared.Events;

public class OrderCompletedEvent : IntegrationEvent
{
    public int OrderId { get; set; }
}

public class OrderFailedEvent : IntegrationEvent
{
    public int OrderId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string FailedAtStage { get; set; } = string.Empty;
}
