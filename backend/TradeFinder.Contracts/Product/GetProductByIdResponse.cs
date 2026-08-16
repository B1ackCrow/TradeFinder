namespace TradeFinder.Contracts.Product;

public class GetProductByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public Guid? ParentId { get; set; }
}