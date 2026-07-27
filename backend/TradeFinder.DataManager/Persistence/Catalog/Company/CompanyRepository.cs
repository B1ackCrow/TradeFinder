using Dapper;
using System.Text.Json;
using Npgsql;

namespace TradeFinder.DataManager;

public class CompanyRepository : RepositoryBase, ICompanyRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public CompanyRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Company>> GetAll()
    {
        const string sql = """
                            SELECT *
                            FROM catalog.vw_companies
                           """;
        
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        
        await using var command = new NpgsqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        List<Company> result = [];

        while (await reader.ReadAsync())
        {
            result.Add(ReadCompany(reader));
        }
        
        return result;
    }

    
    public async Task<Company?> GetById(Guid id)
    {
        const string sql = """
                           SELECT *
                           FROM catalog.vw_companies
                           WHERE id = @Id;
                           """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("Id", id);

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return ReadCompany(reader);
    }
    
    private static Company ReadCompany(NpgsqlDataReader reader)
    {
        return new Company
        {
            Id = reader.GetGuid(reader.GetOrdinal("id")),
            Name = reader.GetString(reader.GetOrdinal("name")),
            Description = GetNullableString(reader, "description"),
            Image = GetNullableString(reader, "image"),

            Locations = DeserializeList<Location>(reader, "locations"),
            Contacts = DeserializeList<CompanyContact>(reader, "contacts"),
            Products = DeserializeList<Product>(reader, "products")
        };
    }
    
    
    private static List<T> DeserializeList<T>(NpgsqlDataReader reader, string column)
    {
        var ordinal = reader.GetOrdinal(column);

        if (reader.IsDBNull(ordinal))
            return [];

        var json = reader.GetString(ordinal);

        return JsonSerializer.Deserialize<List<T>>(json) ?? [];
    }
}