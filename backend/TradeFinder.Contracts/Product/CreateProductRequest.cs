namespace TradeFinder.Contracts.Product;

public class CreateProductRequest
{
    public string Name { get; set; }
    public string? Image { get; set; }
    public Guid? ParentProductId { get; set; }
}