namespace TradeFinder.DataManager;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Image { get; set; }
    public Guid? ParentProductId { get; set; }
}