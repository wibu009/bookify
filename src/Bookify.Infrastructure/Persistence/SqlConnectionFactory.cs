using System.Data;
using Bookify.Application.Abstractions.Persistent;
using Npgsql;

namespace Bookify.Infrastructure.Persistence;

internal sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        return connection;
    }
}