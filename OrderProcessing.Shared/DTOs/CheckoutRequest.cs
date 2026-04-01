namespace OrderProcessing.Shared.DTOs;

public class CheckoutRequest
{
    public int CustomerId { get; set; }
    public List<CartItemRequest> Items { get; set; } = new();
}

public class CartItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
