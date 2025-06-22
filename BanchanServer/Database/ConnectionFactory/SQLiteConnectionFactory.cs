using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace BanchanServer.Database.ConnectionFactory;

public class SQLiteConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SQLiteConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found.");
    }

    public DbConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }
}