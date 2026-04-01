using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using OrderProcessing.Api.Data;
using OrderProcessing.Api.Mapping;

namespace OrderProcessing.Tests.Helpers;

public static class TestDbContextFactory
{
    public static OrderProcessingDbContext Create(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<OrderProcessingDbContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options;

        var context = new OrderProcessingDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}

public static class TestMapperFactory
{
    public static IMapper Create()
    {
        var config = CreateConfiguration();
        return config.CreateMapper();
    }

    public static MapperConfiguration CreateConfiguration()
    {
        var expr = new MapperConfigurationExpression();
        expr.AddProfile<MappingProfile>();
        return new MapperConfiguration(expr, NullLoggerFactory.Instance);
    }
}
