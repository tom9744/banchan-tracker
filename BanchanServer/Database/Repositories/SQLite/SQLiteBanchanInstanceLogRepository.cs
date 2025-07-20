
using BanchanServer.Models.Entities;
using BanchanServer.Database.ConnectionFactory;
using BanchanServer.Database.Repositories.Interfaces;
using Microsoft.Data.Sqlite;

namespace BanchanServer.Database.Repositories.SQLite;

public class SQLiteBanchanInstanceLogRepository(IDbConnectionFactory connectionFactory) : IBanchanInstanceLogRepository
{
    private readonly IDbConnectionFactory _connectionFactory = connectionFactory;

    public async Task AddAsync(BanchanInstanceLog log)
    {
        using var conn = _connectionFactory.CreateConnection();

        if (conn is not SqliteConnection sqliteConn)
        {
            throw new InvalidOperationException("이 저장소는 SQLite 연결만 지원합니다.");
        }

        await sqliteConn.OpenAsync();

        var command = sqliteConn.CreateCommand();
        command.CommandText = """
            INSERT INTO BanchanInstanceLog (Id, BanchanInstanceId, Type, DetailJson, LoggedAt) 
            VALUES (@Id, @BanchanInstanceId, @Type, @DetailJson, @LoggedAt)
        """;
        command.Parameters.AddWithValue("@Id", log.Id);
        command.Parameters.AddWithValue("@BanchanInstanceId", log.BanchanInstanceId);
        command.Parameters.AddWithValue("@Type", LogTypeToString(log.Type));
        command.Parameters.AddWithValue("@DetailJson", log.DetailJson);
        command.Parameters.AddWithValue("@LoggedAt", log.LoggedAt.ToString("yyyy-MM-dd HH:mm:ss"));

        await command.ExecuteNonQueryAsync();
    }

    public async Task<BanchanInstanceLog?> GetByIdAsync(string id)
    {
        using var conn = _connectionFactory.CreateConnection();

        if (conn is not SqliteConnection sqliteConn)
        {
            throw new InvalidOperationException("이 저장소는 SQLite 연결만 지원합니다.");
        }

        await sqliteConn.OpenAsync();

        var command = sqliteConn.CreateCommand();
        command.CommandText = "SELECT * FROM BanchanInstanceLog WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new BanchanInstanceLog
            {
                Id = reader.GetString(0),
                BanchanInstanceId = reader.GetString(1),
                Type = StringToLogType(reader.GetString(2)),
                DetailJson = reader.GetString(3),
                LoggedAt = reader.GetDateTime(4)
            };
        }
        return null;
    }

    public async Task<IEnumerable<BanchanInstanceLog>> GetByInstanceIdAsync(string banchanInstanceId)
    {
        using var conn = _connectionFactory.CreateConnection();

        if (conn is not SqliteConnection sqliteConn)
        {
            throw new InvalidOperationException("이 저장소는 SQLite 연결만 지원합니다.");
        }

        await sqliteConn.OpenAsync();

        var command = sqliteConn.CreateCommand();
        command.CommandText = "SELECT * FROM BanchanInstanceLog WHERE BanchanInstanceId = @BanchanInstanceId";
        command.Parameters.AddWithValue("@BanchanInstanceId", banchanInstanceId);

        using var reader = await command.ExecuteReaderAsync();

        var logs = new List<BanchanInstanceLog>();

        while (await reader.ReadAsync())
        {
            var log = new BanchanInstanceLog
            {
                Id = reader.GetString(0),
                BanchanInstanceId = reader.GetString(1),
                Type = StringToLogType(reader.GetString(2)),
                DetailJson = reader.GetString(3),
                LoggedAt = reader.GetDateTime(4)
            };

            logs.Add(log);
        }

        return logs;
    }

    private string LogTypeToString(LogType logType)
    {
        return logType switch
        {
            LogType.Consumption => "Consumption",
            LogType.Disposal => "Disposal",
            _ => throw new ArgumentException($"Unknown LogType: {logType}")
        };
    }

    private LogType StringToLogType(string value)
    {
        return value switch
        {
            "Consumption" => LogType.Consumption,
            "Disposal" => LogType.Disposal,
            _ => throw new ArgumentException($"Unknown LogType string: {value}")
        };
    }
}