namespace TradeFinder.DataManager;

public class CompanyContact
{
    public Guid Id { get; set; }
    public CompanyContactType CompanyContactType { get; set; }
    public string Value { get; set; }
    public bool IsActive { get; set; }
}