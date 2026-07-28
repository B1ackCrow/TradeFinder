namespace TradeFinder.DataManager;

public class Location
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string TimeZoneCode { get; set; }
    public Guid? ParentLocationId { get; set; }
    public Guid LocationTypeId { get; set; }
}