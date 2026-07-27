using Dapper;
using System.Text.Json;
namespace TradeFinder.DataManager;

public class CompanyRepository : ICompanyRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public CompanyRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Company>> GetCompanies()
    {
        const string sql = """
                            SELECT *
                            FROM catalog.vw_companies
                           """;
        
        using var connection = _connectionFactory.Create();
        var rows = await connection.QueryAsync<dynamic>(sql);
        var companies = new List<Company>();
        foreach (var row in rows)
        {
            var company = new Company();
            company.Id = row.id;
            company.Name = row.name;
            company.Description = row.description;
            // Deserialize JSON columns, default to empty list if null
            company.Locations = row.locations != null
                ? JsonSerializer.Deserialize<List<Location>>(row.locations.ToString()) ?? new List<Location>()
                : new List<Location>();
            company.Contacts = row.contacts != null
                ? JsonSerializer.Deserialize<List<Contact>>(row.contacts.ToString()) ?? new List<Contact>()
                : new List<Contact>();
            company.Products = row.products != null
                ? JsonSerializer.Deserialize<List<Product>>(row.products.ToString()) ?? new List<Product>()
                : new List<Product>();
            companies.Add(company);
        }
        return companies;
    }

}