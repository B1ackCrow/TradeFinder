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

    [HttpPost("import-from-json")]
    public async Task<IActionResult> ImportFromJson(string filePath)
    {
        try
        {
            await _productService.ImportFromJson(filePath);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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

    [HttpGet("get-by-id")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            GetProductByIdResponse product = await _productService.GetProductById(id);
            return Ok(product);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromForm] CreateProductRequest request)
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
    
    [HttpPost("update")]
    public async Task<IActionResult> Update([FromForm] UpdateProductRequest request)
    {
        try
        {
            await _productService.UpdateProduct(request.Id, request.Name, request.Image, request.ParentProductId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}