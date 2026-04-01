using AutoMapper;
using OrderProcessing.Shared.DTOs;
using OrderProcessing.Shared.Entities;
using OrderProcessing.Shared.Enums;
using OrderProcessing.Tests.Helpers;

namespace OrderProcessing.Tests.Mapping;

public class MappingProfileTests
{
    private readonly IMapper _mapper = TestMapperFactory.Create();

    [Fact]
    public void MappingProfile_Configuration_IsValid()
    {
        var config = TestMapperFactory.CreateConfiguration();
        config.AssertConfigurationIsValid();
    }

    [Fact]
    public void Product_Maps_To_ProductDto()
    {
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "A test product",
            Price = 29.99m,
            Category = "Test",
            StockQuantity = 50
        };

        var dto = _mapper.Map<ProductDto>(product);

        Assert.Equal(product.Id, dto.Id);
        Assert.Equal(product.Name, dto.Name);
        Assert.Equal(product.Price, dto.Price);
        Assert.Equal(product.Category, dto.Category);
        Assert.Equal(product.StockQuantity, dto.StockQuantity);
    }

    [Fact]
    public void Order_Maps_To_OrderDto_With_Status_String()
    {
        var order = new Order
        {
            Id = 1,
            CustomerId = 1,
            Customer = new Customer { Id = 1, Name = "Alice", Email = "a@b.com" },
            Status = OrderStatus.Completed,
            TotalAmount = 100m,
            OrderDate = DateTime.UtcNow,
            Items = new List<OrderItem>()
        };

        var dto = _mapper.Map<OrderDto>(order);

        Assert.Equal("Completed", dto.Status);
        Assert.Equal("Alice", dto.CustomerName);
        Assert.Equal(100m, dto.TotalAmount);
    }

    [Fact]
    public void Order_Maps_To_OrderStatusDto()
    {
        var order = new Order
        {
            Id = 5,
            Status = OrderStatus.PaymentPending,
            OrderDate = DateTime.UtcNow
        };

        var dto = _mapper.Map<OrderStatusDto>(order);

        Assert.Equal(5, dto.OrderId);
        Assert.Equal("PaymentPending", dto.Status);
    }

    [Fact]
    public void Customer_Maps_To_CustomerDto()
    {
        var customer = new Customer
        {
            Id = 1,
            Name = "Bob",
            Email = "bob@test.com",
            City = "Dublin",
            Country = "Ireland"
        };

        var dto = _mapper.Map<CustomerDto>(customer);

        Assert.Equal("Bob", dto.Name);
        Assert.Equal("bob@test.com", dto.Email);
        Assert.Equal("Dublin", dto.City);
    }
}
