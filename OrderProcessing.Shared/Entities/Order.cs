using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OrderProcessing.Shared.Enums;

namespace OrderProcessing.Shared.Entities;

public class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public OrderStatus Status { get; set; } = OrderStatus.Cart;

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    public string? FailureReason { get; set; }

    public string CorrelationId { get; set; } = Guid.NewGuid().ToString();

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    public PaymentRecord? PaymentRecord { get; set; }
    public ShipmentRecord? ShipmentRecord { get; set; }
}
