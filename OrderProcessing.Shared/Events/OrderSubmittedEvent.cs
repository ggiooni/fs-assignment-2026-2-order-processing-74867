namespace OrderProcessing.Shared.Events;

public class OrderSubmittedEvent : IntegrationEvent
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public List<OrderItemEvent> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
}

public class OrderItemEvent
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
