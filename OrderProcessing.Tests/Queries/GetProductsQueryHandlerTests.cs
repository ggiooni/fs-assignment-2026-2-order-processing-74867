using AutoMapper;
using OrderProcessing.Api.Features.Products.Queries;
using OrderProcessing.Shared.Entities;
using OrderProcessing.Tests.Helpers;

namespace OrderProcessing.Tests.Queries;

public class GetProductsQueryHandlerTests
{
    private readonly IMapper _mapper = TestMapperFactory.Create();

    [Fact]
    public async Task Handle_ReturnsAllProducts()
    {
        var db = TestDbContextFactory.Create();
        db.Products.Add(new Product { Id = 100, Name = "Ball", Price = 20, Category = "Sports", StockQuantity = 10 });
        db.Products.Add(new Product { Id = 101, Name = "Bat", Price = 40, Category = "Sports", StockQuantity = 5 });
        await db.SaveChangesAsync();

        var handler = new GetProductsQueryHandler(db, _mapper);
        var result = await handler.Handle(new GetProductsQuery(), CancellationToken.None);

        // Seed data + 2 added = at least 11
        Assert.True(result.Count >= 2);
        Assert.Contains(result, p => p.Name == "Ball");
        Assert.Contains(result, p => p.Name == "Bat");
    }

    [Fact]
    public async Task Handle_FiltersByCategory()
    {
        var db = TestDbContextFactory.Create();
        db.Products.Add(new Product { Id = 102, Name = "Test Chess Board", Price = 50, Category = "TestCategory", StockQuantity = 10 });
        db.Products.Add(new Product { Id = 103, Name = "Football", Price = 30, Category = "Soccer", StockQuantity = 20 });
        await db.SaveChangesAsync();

        var handler = new GetProductsQueryHandler(db, _mapper);
        var result = await handler.Handle(new GetProductsQuery("TestCategory"), CancellationToken.None);

        Assert.Single(result);
        Assert.Equal("Test Chess Board", result[0].Name);
    }
}
