using Microsoft.Extensions.Configuration;
using Npgsql;

namespace TradeFinder.DataManager;

public class ConnectionFactory
{
    private readonly string _connectionString;

    public ConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ConnectionString")
                            ?? throw new InvalidOperationException("Не удалось получить строку подключения 'ConnectionString'.");
    }

    public NpgsqlConnection Create()
    {
        return new NpgsqlConnection(_connectionString);
    }
}