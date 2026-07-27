using Npgsql;

namespace TradeFinder.DataManager;

public class ProductRepository : IProductRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public ProductRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> Create(Product product)
    {
        const string sql = """
                           INSERT INTO catalog.products (name, description, image)
                           values (@Name, @Description, @Image)
                           RETURNING id;
                           """;
        
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        
        await using var command = new NpgsqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("Name", product.Name);
        command.Parameters.AddWithValue("Description", (object?)product.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("Image", (object?)product.Image ?? DBNull.Value);
        
        return (Guid)(await command.ExecuteScalarAsync())!;
    }

    public async Task Update(Product product)
    {
        const string sql = """
                            UPDATE catalog.products
                            SET
                                name = @Name,
                                description = @Description,
                                image = @Image
                            WHERE id = @Id;
                           """;
        
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        
        await using var command = new NpgsqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("Id", product.Id);
        command.Parameters.AddWithValue("Name", product.Name);
        command.Parameters.AddWithValue("Description", (object?)product.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("Image", (object?)product.Image ?? DBNull.Value);

        await command.ExecuteNonQueryAsync();
    }
    
    public async Task<List<Product>> GetAll()
    {
        const string sql = "SELECT * FROM catalog.products;";

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        List <Product> result = new();

        while (await reader.ReadAsync())
        {
            result.Add(ReadProduct(reader));
        }
        
        return result;
    }

    public async Task<Product?> GetById(Guid id)
    {
        const string sql = """
                           SELECT *
                           FROM catalog.products
                           WHERE id = @Id;
                           """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("Id", id);

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return ReadProduct(reader);
    }
    
    public async Task<bool> ExistsByName(Guid id, string name)
    {
        const string sql = """
                            SELECT EXISTS 
                            (
                                SELECT 1
                                FROM catalog.products
                                WHERE lower(name) = lower(@Name)
                                AND id <> @Id
                            );
                           """;
        
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("Name", name);
        command.Parameters.AddWithValue("Id", id);

        var result = await command.ExecuteScalarAsync();

        return (bool)result!;   
    }
    
    public async Task<bool> ExistsByName(string name)
    {
        const string sql = """
                            SELECT EXISTS 
                            (
                                SELECT 1
                                FROM catalog.products
                                WHERE lower(name) = lower(@Name)
                            );
                           """;
        
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("Name", name);

        var result = await command.ExecuteScalarAsync();

        return (bool)result!;   
    }
    
    
    protected static string? GetNullableString(NpgsqlDataReader reader, string column)
    {
        var ordinal = reader.GetOrdinal(column);
        
        return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
    }
    
    private static Product ReadProduct(NpgsqlDataReader reader)
    {
        return new Product
        {
            Id = reader.GetGuid(reader.GetOrdinal("id")),
            Name = reader.GetString(reader.GetOrdinal("name")),
            Description = GetNullableString(reader, "description"),
            Image = GetNullableString(reader, "image")
        };
    }
}