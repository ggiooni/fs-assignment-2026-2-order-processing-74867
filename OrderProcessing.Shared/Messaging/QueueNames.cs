namespace OrderProcessing.Shared.Messaging;

public static class QueueNames
{
    public const string OrderSubmitted = "order-submitted";
    public const string InventoryConfirmed = "inventory-confirmed";
    public const string InventoryFailed = "inventory-failed";
    public const string PaymentApproved = "payment-approved";
    public const string PaymentFailed = "payment-failed";
    public const string ShippingCreated = "shipping-created";
    public const string ShippingFailed = "shipping-failed";
    public const string OrderCompleted = "order-completed";
    public const string OrderFailed = "order-failed";
}
