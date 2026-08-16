using Microsoft.AspNetCore.Http;

namespace TradeFinder.Contracts.Product;

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
    public Guid? ParentProductId { get; set; }
}