using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Api.Features.Products.Queries;

namespace OrderProcessing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? category = null)
    {
        var products = await _mediator.Send(new GetProductsQuery(category));
        return Ok(products);
    }
}
