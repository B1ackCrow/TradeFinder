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

    public async Task<Guid> Create(Company company)
    {
        const string sql = """
                            INSERT INTO catalog.companies (name, description, inn, logo)
                            VALUES (@Name, @Description, @Inn, @Logo)
                            RETURNING id;
                           """;
        
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        
        await using var command = new NpgsqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("Name", company.Name);
        command.Parameters.AddWithValue("Description", (object?)company.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("Inn", (object?)company.Inn ?? DBNull.Value);
        command.Parameters.AddWithValue("Logo", (object?)company.Logo ?? DBNull.Value);
        
        return (Guid)(await command.ExecuteScalarAsync())!;
    }

    public async Task Update(Company company)
    {
        const string sql = """
                            UPDATE catalog.companies
                            SET
                                name = @Name,
                                description = @Description,
                                inn = @Inn,
                                logo = @Logo
                           WHERE id = @Id;
                           """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        
        await using var command = new NpgsqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("Id", company.Id);
        command.Parameters.AddWithValue("Name", company.Name);
        command.Parameters.AddWithValue("Description", (object?)company.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("Inn", (object?)company.Inn ?? DBNull.Value);
        command.Parameters.AddWithValue("Logo", (object?)company.Logo ?? DBNull.Value);
        
        await command.ExecuteNonQueryAsync();
    }

    public async Task Delete(Guid id)
    {
        const string sql = """
                            DELETE FROM catalog.companies
                            WHERE id = @Id;
                           """;
        
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("Id", id);
        await command.ExecuteNonQueryAsync();
    }
    
    public async Task CreateProduct(Guid companyId, Guid productId)
    {
        const string sql = """
                            INSERT INTO catalog.company_products (company_id, product_id)
                            VALUES (@CompanyId, @ProductId)
                           """;
        
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        
        await using var command = new NpgsqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("CompanyId", companyId);
        command.Parameters.AddWithValue("ProductId", productId);
        
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteProduct(Guid companyId, Guid productId)
    {
        const string sql = """  
                            DELETE FROM catalog.company_products
                            WHERE company_id = @CompanyId
                            AND product_id = @ProductId;
                           """;
        
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("CompanyId", companyId);
        command.Parameters.AddWithValue("ProductId", productId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task CreateContact(Guid companyId, Guid companyContactTypeId, string value)
    {
        const string sql = """
                            INSERT INTO catalog.company_contacts (company_id, company_contact_type_id, value)
                            VALUES (@CompanyId, @CompanyContactTypeId, @Value)
                           """;
        
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("CompanyId", companyId);
        command.Parameters.AddWithValue("CompanyContactTypeId", companyContactTypeId);
        command.Parameters.AddWithValue("Value", value);
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteContact(Guid companyContactId)
    {
        const string sql = """
                            DELETE FROM catalog.company_contacts
                            WHERE id = @CompanyContactId;
                           """;
        
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("CompanyContactId", companyContactId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task CreateLocation(Guid companyId, Guid locationId)
    {
        const string sql = """
                            INSERT INTO location.company_locations (company_id, location_id)
                            VALUES (@CompanyId, @LocationId)
                           """;
        
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        
        await using var command = new NpgsqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("CompanyId", companyId);
        command.Parameters.AddWithValue("LocationId", locationId);
        
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteLocation(Guid companyId, Guid locationId)
    {
        const string sql = """  
                            DELETE FROM location.company_locations
                            WHERE company_id = @CompanyId
                            AND location_id = @LocationId;
                           """;
        
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("CompanyId", companyId);
        command.Parameters.AddWithValue("LocationId", locationId);
        await command.ExecuteNonQueryAsync();
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

    public async Task<Company?> GetById(Guid companyId)
    {
        const string sql = """
                            SELECT *
                            FROM catalog.vw_companies
                            WHERE id = @CompanyId;
                           """;
        
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("CompanyId", companyId);
        
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }
        
        return ReadCompany(reader);
    }
    
    private Company ReadCompany(NpgsqlDataReader reader)
    {
        return new Company
        {
            Id = reader.GetGuid(reader.GetOrdinal("id")),
            Name = reader.GetString(reader.GetOrdinal("name")),

            Description = reader.IsDBNull(reader.GetOrdinal("description"))
                ? null
                : reader.GetString(reader.GetOrdinal("description")),

            Logo = reader.IsDBNull(reader.GetOrdinal("logo"))
                ? null
                : reader.GetString(reader.GetOrdinal("logo")),

            Inn = reader.IsDBNull(reader.GetOrdinal("inn"))
                ? null
                : reader.GetString(reader.GetOrdinal("inn")),

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