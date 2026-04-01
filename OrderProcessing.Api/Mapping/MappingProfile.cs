using AutoMapper;
using OrderProcessing.Shared.DTOs;
using OrderProcessing.Shared.Entities;

namespace OrderProcessing.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<Customer, CustomerDto>();

        CreateMap<Order, OrderDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.Name));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name));

        CreateMap<Order, OrderStatusDto>()
            .ForMember(d => d.OrderId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<PaymentRecord, PaymentRecordDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<ShipmentRecord, ShipmentRecordDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
    }
}
