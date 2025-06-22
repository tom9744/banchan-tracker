using System.Data.Common;

namespace BanchanServer.Database.ConnectionFactory;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();
}