using System.ComponentModel.DataAnnotations.Schema;
using OrderProcessing.Shared.Enums;

namespace OrderProcessing.Shared.Entities;

public class PaymentRecord
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public string? TransactionId { get; set; }

    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
}
