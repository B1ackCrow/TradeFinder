namespace TradeFinder.Contracts.Product;

public class GetAllProductsResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<GetAllProductsResponse> Children { get; set; } = [];
}