using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Api.Features.Orders.Commands;
using OrderProcessing.Api.Features.Orders.Queries;
using OrderProcessing.Shared.DTOs;

namespace OrderProcessing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
    {
        var command = new SubmitOrderCommand(request.CustomerId, request.Items);
        var order = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? status = null)
    {
        var orders = await _mediator.Send(new GetOrdersQuery(status));
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(id));
        return order is null ? NotFound() : Ok(order);
    }

    [HttpGet("{id}/status")]
    public async Task<IActionResult> GetStatus(int id)
    {
        var status = await _mediator.Send(new GetOrderStatusQuery(id));
        return status is null ? NotFound() : Ok(status);
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var summary = await _mediator.Send(new GetDashboardSummaryQuery());
        return Ok(summary);
    }
}
