using Npgsql;

namespace TradeFinder.DataManager;

public class ProductRepository : RepositoryBase, IProductRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public ProductRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> Create(Product product)
    {
        const string sql = """
                           INSERT INTO catalog.products
                           (
                               name,
                               image,
                               okpd2_code,
                               classifier_code,
                               unit,
                               parent_product_id
                           )
                           VALUES
                           (
                               @Name,
                               @Image,
                               @Okpd2Code,
                               @ClassifierCode,
                               @Unit,
                               @ParentProductId
                           )
                           RETURNING id;
                           """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("Name", product.Name);
        command.Parameters.AddWithValue("Image", (object?)product.Image ?? DBNull.Value);
        command.Parameters.AddWithValue("Okpd2Code", (object?)product.Okpd2Code ?? DBNull.Value);
        command.Parameters.AddWithValue("ClassifierCode", (object?)product.ClassifierCode ?? DBNull.Value);
        command.Parameters.AddWithValue("Unit", (object?)product.Unit ?? DBNull.Value);
        command.Parameters.AddWithValue("ParentProductId", (object?)product.ParentProductId ?? DBNull.Value);

        return (Guid)(await command.ExecuteScalarAsync())!;
    }

    public async Task Update(Product product)
    {
        const string sql = """
                           UPDATE catalog.products
                           SET
                               name = @Name,
                               image = @Image,
                               okpd2_code = @Okpd2Code,
                               classifier_code = @ClassifierCode,
                               unit = @Unit,
                               parent_product_id = @ParentProductId
                           WHERE id = @Id;
                           """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("Id", product.Id);
        command.Parameters.AddWithValue("Name", product.Name);
        command.Parameters.AddWithValue("Image", (object?)product.Image ?? DBNull.Value);
        command.Parameters.AddWithValue("Okpd2Code", (object?)product.Okpd2Code ?? DBNull.Value);
        command.Parameters.AddWithValue("ClassifierCode", (object?)product.ClassifierCode ?? DBNull.Value);
        command.Parameters.AddWithValue("Unit", (object?)product.Unit ?? DBNull.Value);
        command.Parameters.AddWithValue("ParentProductId", (object?)product.ParentProductId ?? DBNull.Value);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<Product>> GetAll()
    {
        const string sql = """
                           SELECT
                               id,
                               name,
                               image,
                               okpd2_code,
                               classifier_code,
                               unit,
                               parent_product_id
                           FROM catalog.products;
                           """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        List<Product> result = new();

        while (await reader.ReadAsync())
        {
            result.Add(ReadProduct(reader));
        }

        return result;
    }

    public async Task<Product?> GetById(Guid id)
    {
        const string sql = """
                           SELECT
                               id,
                               name,
                               image,
                               okpd2_code,
                               classifier_code,
                               unit,
                               parent_product_id
                           FROM catalog.products
                           WHERE id = @Id;
                           """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("Id", id);

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

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

    private static Product ReadProduct(NpgsqlDataReader reader)
    {
        return new Product
        {
            Id = reader.GetGuid(reader.GetOrdinal("id")),
            Name = reader.GetString(reader.GetOrdinal("name")),
            Image = GetNullableString(reader, "image"),
            Okpd2Code = GetNullableString(reader, "okpd2_code"),
            ClassifierCode = GetNullableString(reader, "classifier_code"),
            Unit = GetNullableString(reader, "unit"),
            ParentProductId = GetNullableGuid(reader, "parent_product_id")
        };
    }
}