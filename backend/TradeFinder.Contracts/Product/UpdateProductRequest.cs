using Microsoft.AspNetCore.Http;

namespace TradeFinder.Contracts.Product;

public class UpdateProductRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IFormFile? Image { get; set; } = null;
    public Guid? ParentProductId { get; set; } = null;
}