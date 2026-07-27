using Npgsql;

namespace TradeFinder.DataManager;

public class LocationRepository : RepositoryBase, ILocationRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public LocationRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Location>> GetByType(string  locationTypeCode, Guid? parentLocationId = null) 
    {
        const string sql = """
                            SELECT *
                            FROM location.locations l
                            JOIN location.location_types lt
                                ON lt.id = l.location_type_id
                            WHERE lt.code = @Code
                            AND (
                                @ParentLocationId IS NULL
                                OR l.parent_location_id = @ParentLocationId
                            )
                            ORDER BY l.name;
                           """;
        
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("Code", locationTypeCode);
        command.Parameters.AddWithValue("ParentLocationId", (object?)parentLocationId ?? DBNull.Value);
        
        await using var reader = await command.ExecuteReaderAsync();

        List <Location> result = new();

        while (await reader.ReadAsync())
        {
            result.Add(ReadLocation(reader));
        }
        
        return result;
    }
    
    private static Location ReadLocation(NpgsqlDataReader reader)
    {
        return new Location
        {
            Id = reader.GetGuid(reader.GetOrdinal("id")),
            Name = reader.GetString(reader.GetOrdinal("name")),
            ParentLocationId = GetNullableGuid(reader, "parent_location_id"),
            TimeZoneCode =  reader.GetString(reader.GetOrdinal("timezone_code")),
            LocationTypeId = reader.GetGuid(reader.GetOrdinal("location_type_id"))
        };
    }
}