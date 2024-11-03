using System.Data;
using Npgsql;

namespace ClickerC3p0.Database;

public class NpgsqlDbConnectionFactory: IDBConnectionFactory
{
    private readonly string _connectionString;

    public NpgsqlDbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}


public interface IDBConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}