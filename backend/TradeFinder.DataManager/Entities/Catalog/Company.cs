namespace TradeFinder.DataManager;

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public List<Location> Locations { get; set; }
    public List<CompanyContact> Contacts { get; set; }
    public List<Product> Products { get; set; }
}