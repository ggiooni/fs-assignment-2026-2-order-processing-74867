namespace OrderProcessing.Shared.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string? FailureReason { get; set; }
    public string CorrelationId { get; set; } = string.Empty;
    public List<OrderItemDto> Items { get; set; } = new();
    public PaymentRecordDto? PaymentRecord { get; set; }
    public ShipmentRecordDto? ShipmentRecord { get; set; }
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class OrderStatusDto
{
    public int OrderId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string? FailureReason { get; set; }
}

public class PaymentRecordDto
{
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? TransactionId { get; set; }
    public DateTime ProcessedAt { get; set; }
}

public class ShipmentRecordDto
{
    public string? TrackingNumber { get; set; }
    public string Carrier { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? ShippedAt { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
}
