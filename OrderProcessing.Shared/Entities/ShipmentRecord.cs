using OrderProcessing.Shared.Enums;

namespace OrderProcessing.Shared.Entities;

public class ShipmentRecord
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public string? TrackingNumber { get; set; }

    public string Carrier { get; set; } = "Standard Shipping";

    public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;

    public DateTime? ShippedAt { get; set; }

    public DateTime? EstimatedDelivery { get; set; }
}
