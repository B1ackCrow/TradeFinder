using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeFinder.Business.Product;
using TradeFinder.Contracts.Product;

namespace TradeFinder.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly ProductService _productService;

    public ProductsController(ILogger<ProductsController> logger,  ProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            List<GetAllProductsResponse> allProductsResponse = await _productService.GetAllProducts();
            return Ok(allProductsResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        try
        {
            Guid id = await _productService.CreateProduct(request.Name, request.Image, request.ParentProductId);
            return Ok(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}