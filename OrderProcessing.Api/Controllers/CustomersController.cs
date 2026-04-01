using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Api.Features.Orders.Queries;

namespace OrderProcessing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}/orders")]
    public async Task<IActionResult> GetOrders(int id)
    {
        var orders = await _mediator.Send(new GetCustomerOrdersQuery(id));
        return Ok(orders);
    }
}
