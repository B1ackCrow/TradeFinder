using Npgsql;

namespace TradeFinder.DataManager;

public class AuthRepository : RepositoryBase, IAuthRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public AuthRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByEmail(string email)
    {
        const string sql = """
                           SELECT *
                           FROM auth.users
                           WHERE lower(email) = lower(@Email);
                           """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("Email", email);

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return ReadUser(reader);
    }

    public async Task<User?> GetByLogin(string login)
    {
        const string sql = """
                           SELECT *
                           FROM auth.users
                           WHERE lower(login) = lower(@Login);
                           """;

        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("Login", login);

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return ReadUser(reader);
    }

    public async Task<Guid> Create(User user)
    {
        const string sql = """
                           INSERT INTO auth.users (email, password_hash)
                           values (@Email, @PasswordHash)
                           RETURNING id;
                           """;
        
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        
        await using var command = new NpgsqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("Email", user.Email);
        command.Parameters.AddWithValue("PasswordHash", user.PasswordHash);
        
        return (Guid)(await command.ExecuteScalarAsync())!;
    }

    public async Task<bool> ExistsByEmail(string email)
    {
        const string sql = """
                            SELECT EXISTS
                            (
                                SELECT 1
                                FROM auth.users
                                WHERE lower(email) = lower(@Email)
                            );
                           """;
        
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        
        await using var command = new NpgsqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("Email", email);

        return (bool)(await command.ExecuteScalarAsync())!;
    }
    
    private static User ReadUser(NpgsqlDataReader reader)
    {
        return new User()
        {
            Id = reader.GetGuid(reader.GetOrdinal("id")),
            Login = GetNullableString(reader, "login"),
            Email = reader.GetString(reader.GetOrdinal("email")),
            PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
            IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
            UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at")),
        };
    }
}