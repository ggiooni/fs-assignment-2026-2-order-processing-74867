using Microsoft.EntityFrameworkCore;
using OrderProcessing.Shared.Entities;

namespace OrderProcessing.Api.Data;

public class OrderProcessingDbContext : DbContext
{
    public OrderProcessingDbContext(DbContextOptions<OrderProcessingDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<InventoryRecord> InventoryRecords => Set<InventoryRecord>();
    public DbSet<PaymentRecord> PaymentRecords => Set<PaymentRecord>();
    public DbSet<ShipmentRecord> ShipmentRecords => Set<ShipmentRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId);

        modelBuilder.Entity<PaymentRecord>()
            .HasOne(p => p.Order)
            .WithOne(o => o.PaymentRecord)
            .HasForeignKey<PaymentRecord>(p => p.OrderId);

        modelBuilder.Entity<ShipmentRecord>()
            .HasOne(s => s.Order)
            .WithOne(o => o.ShipmentRecord)
            .HasForeignKey<ShipmentRecord>(s => s.OrderId);

        modelBuilder.Entity<InventoryRecord>()
            .HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId);

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.Status);

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.CustomerId);

        // Seed Products
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Kayak", Description = "A boat for one person", Price = 275.00m, Category = "Watersports", StockQuantity = 50 },
            new Product { Id = 2, Name = "Lifejacket", Description = "Protective and fashionable", Price = 48.95m, Category = "Watersports", StockQuantity = 100 },
            new Product { Id = 3, Name = "Soccer Ball", Description = "FIFA-approved size and weight", Price = 19.50m, Category = "Soccer", StockQuantity = 200 },
            new Product { Id = 4, Name = "Corner Flags", Description = "Give your pitch a professional touch", Price = 34.95m, Category = "Soccer", StockQuantity = 80 },
            new Product { Id = 5, Name = "Stadium", Description = "Flat-packed 35,000-seat stadium", Price = 79500.00m, Category = "Soccer", StockQuantity = 2 },
            new Product { Id = 6, Name = "Thinking Cap", Description = "Improve brain efficiency by 75%", Price = 16.00m, Category = "Chess", StockQuantity = 150 },
            new Product { Id = 7, Name = "Unsteady Chair", Description = "Secretly give your opponent a disadvantage", Price = 29.95m, Category = "Chess", StockQuantity = 60 },
            new Product { Id = 8, Name = "Human Chess Board", Description = "A fun game for the family", Price = 75.00m, Category = "Chess", StockQuantity = 25 },
            new Product { Id = 9, Name = "Bling-Bling King", Description = "Gold-plated diamond-studded King", Price = 1200.00m, Category = "Chess", StockQuantity = 10 }
        );

        // Seed Customers
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, Name = "Alice Johnson", Email = "alice@example.com", Address = "123 Main St", City = "Dublin", Country = "Ireland" },
            new Customer { Id = 2, Name = "Bob Smith", Email = "bob@example.com", Address = "456 Oak Ave", City = "Cork", Country = "Ireland" },
            new Customer { Id = 3, Name = "Charlie Brown", Email = "charlie@example.com", Address = "789 Pine Rd", City = "Galway", Country = "Ireland" }
        );

        // Seed Inventory Records
        modelBuilder.Entity<InventoryRecord>().HasData(
            new InventoryRecord { Id = 1, ProductId = 1, QuantityOnHand = 50, ReservedQuantity = 0 },
            new InventoryRecord { Id = 2, ProductId = 2, QuantityOnHand = 100, ReservedQuantity = 0 },
            new InventoryRecord { Id = 3, ProductId = 3, QuantityOnHand = 200, ReservedQuantity = 0 },
            new InventoryRecord { Id = 4, ProductId = 4, QuantityOnHand = 80, ReservedQuantity = 0 },
            new InventoryRecord { Id = 5, ProductId = 5, QuantityOnHand = 2, ReservedQuantity = 0 },
            new InventoryRecord { Id = 6, ProductId = 6, QuantityOnHand = 150, ReservedQuantity = 0 },
            new InventoryRecord { Id = 7, ProductId = 7, QuantityOnHand = 60, ReservedQuantity = 0 },
            new InventoryRecord { Id = 8, ProductId = 8, QuantityOnHand = 25, ReservedQuantity = 0 },
            new InventoryRecord { Id = 9, ProductId = 9, QuantityOnHand = 10, ReservedQuantity = 0 }
        );
    }
}
