using OrderProcessing.Shared.DTOs;

namespace OrderProcessing.BlazorUI.Services;

public class OrderApiClient
{
    private readonly HttpClient _http;

    public OrderApiClient(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("OrderApi");
    }

    public async Task<List<ProductDto>> GetProductsAsync()
        => await _http.GetFromJsonAsync<List<ProductDto>>("api/products") ?? new();

    public async Task<ProductDto?> GetProductAsync(int id)
    {
        var products = await GetProductsAsync();
        return products.FirstOrDefault(p => p.Id == id);
    }

    public async Task<OrderDto?> CheckoutAsync(CheckoutRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/orders/checkout", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<OrderDto>();
    }

    public async Task<List<OrderDto>> GetCustomerOrdersAsync(int customerId)
        => await _http.GetFromJsonAsync<List<OrderDto>>($"api/customers/{customerId}/orders") ?? new();

    public async Task<OrderStatusDto?> GetOrderStatusAsync(int orderId)
        => await _http.GetFromJsonAsync<OrderStatusDto>($"api/orders/{orderId}/status");

    public async Task<OrderDto?> GetOrderAsync(int orderId)
        => await _http.GetFromJsonAsync<OrderDto>($"api/orders/{orderId}");
}
