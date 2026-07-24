namespace TradeFinder.DataManager;

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public List<Location> Locations { get; set; }
    public List<Contact> Contacts { get; set; }
    public List<Product> Products { get; set; }
}

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
}

public class Contact
{
    public Guid Id { get; set; }
    public string TypeName { get; set; }
    public string Value { get; set; }
    public string IconName { get; set; }
}

public class Location
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string TimeZoneCode { get; set; }
    public Guid? ParentLocationId { get; set; }
}