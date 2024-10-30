using System.Data;

namespace Bookify.Application.Abstractions.Persistent;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}