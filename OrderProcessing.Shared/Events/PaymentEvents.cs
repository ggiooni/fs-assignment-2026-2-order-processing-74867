namespace OrderProcessing.Shared.Events;

public class PaymentApprovedEvent : IntegrationEvent
{
    public int OrderId { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class PaymentFailedEvent : IntegrationEvent
{
    public int OrderId { get; set; }
    public string Reason { get; set; } = string.Empty;
}
