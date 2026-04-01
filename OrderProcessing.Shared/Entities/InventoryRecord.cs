using System.ComponentModel.DataAnnotations.Schema;

namespace OrderProcessing.Shared.Entities;

public class InventoryRecord
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int QuantityOnHand { get; set; }
    public int ReservedQuantity { get; set; }

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
