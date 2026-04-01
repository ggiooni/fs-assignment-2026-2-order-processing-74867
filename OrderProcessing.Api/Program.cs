using Microsoft.EntityFrameworkCore;
using OrderProcessing.Api.Data;
using OrderProcessing.Api.Mapping;
using OrderProcessing.Api.Middleware;
using OrderProcessing.Shared.Messaging;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("ServiceName", "OrderProcessing.Api"));

    // Database
    builder.Services.AddDbContext<OrderProcessingDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
            ?? "Data Source=orderprocessing.db"));

    // MediatR
    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

    // AutoMapper
    builder.Services.AddAutoMapper(cfg =>
        cfg.AddProfile<MappingProfile>());

    // RabbitMQ
    builder.Services.Configure<RabbitMqSettings>(
        builder.Configuration.GetSection("RabbitMq"));
    builder.Services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();

    // Controllers + Swagger
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontends", policy =>
        {
            policy.WithOrigins(
                    "http://localhost:5001",
                    "https://localhost:5001",
                    "http://localhost:3000",
                    "http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    var app = builder.Build();

    // Ensure DB created with seed data
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<OrderProcessingDbContext>();
        db.Database.EnsureCreated();
    }

    app.UseSerilogRequestLogging();
    app.UseMiddleware<CorrelationIdMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("AllowFrontends");
    app.MapControllers();

    Log.Information("OrderProcessing API started");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}
