using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TradeFinder.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger)
    {
        _logger = logger;
    }

    [Authorize]
    [HttpGet("get-all")]
    public IActionResult GetAll()
    {
        return Ok(new[]
        {
            new
            {
                Id = Guid.NewGuid(),
                Name = "Бетон"
            }
        });
    }
}