using BanchanServer.Models.Entities;
using BanchanServer.Database.ConnectionFactory;
using BanchanServer.Database.Repositories.Interfaces;
using Microsoft.Data.Sqlite;

namespace BanchanServer.Database.Repositories.SQLite;

public class SQLiteBanchanInstanceRepository(IDbConnectionFactory connectionFactory) : IBanchanInstanceRepository
{
    private readonly IDbConnectionFactory _connectionFactory = connectionFactory;

    public async Task<IEnumerable<BanchanInstance>> GetAllAsync()
    {
        using var conn = _connectionFactory.CreateConnection();

        if (conn is not SqliteConnection sqliteConn)
        {
            throw new InvalidOperationException("이 저장소는 SQLite 연결만 지원합니다.");
        }

        await sqliteConn.OpenAsync();

        var command = sqliteConn.CreateCommand();
        command.CommandText = "SELECT * FROM BanchanInstance";

        using var reader = await command.ExecuteReaderAsync();

        var instances = new List<BanchanInstance>();

        while (await reader.ReadAsync())
        {
            var instance = new BanchanInstance
            {
                Id = reader.GetString(0),
                BanchanId = reader.GetString(1),
                CreatedAt = DateTime.Parse(reader.GetString(2)),
                UpdatedAt = DateTime.Parse(reader.GetString(3)),
                FinishedAt = reader.IsDBNull(4) ? null : DateTime.Parse(reader.GetString(4)),
                RemainingPortion = reader.GetDouble(5),
                Memo = reader.IsDBNull(6) ? null : reader.GetString(6)
            };

            instances.Add(instance);
        }

        return instances;
    }

    public async Task<BanchanInstance?> GetByIdAsync(string id)
    {
        using var conn = _connectionFactory.CreateConnection();

        if (conn is not SqliteConnection sqliteConn)
        {
            throw new InvalidOperationException("이 저장소는 SQLite 연결만 지원합니다.");
        }

        await sqliteConn.OpenAsync();

        var command = sqliteConn.CreateCommand();
        command.CommandText = 
        @"
            SELECT * FROM BanchanInstance 
            WHERE Id = @Id
        ";
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new BanchanInstance
            {
                Id = reader.GetString(0),
                BanchanId = reader.GetString(1),    
                CreatedAt = DateTime.Parse(reader.GetString(2)),
                UpdatedAt = DateTime.Parse(reader.GetString(3)),
                FinishedAt = reader.IsDBNull(4) ? null : DateTime.Parse(reader.GetString(4)),
                RemainingPortion = reader.GetDouble(5),
                Memo = reader.IsDBNull(6) ? null : reader.GetString(6)
            };
        }
        return null;
    }
    
    public async Task AddAsync(BanchanInstance instance)
    {
        using var conn = _connectionFactory.CreateConnection();
    
        if (conn is not SqliteConnection sqliteConn)
        {
            throw new InvalidOperationException("이 저장소는 SQLite 연결만 지원합니다.");
        }

        await sqliteConn.OpenAsync();

        var command = sqliteConn.CreateCommand();
        command.CommandText = 
        @"
            INSERT INTO BanchanInstance (Id, BanchanId, CreatedAt, Memo) 
            VALUES (@Id, @BanchanId, @CreatedAt, @Memo)
        ";
        command.Parameters.AddWithValue("@Id", instance.Id);
        command.Parameters.AddWithValue("@BanchanId", instance.BanchanId);
        command.Parameters.AddWithValue("@CreatedAt", instance.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@Memo", instance.Memo ?? string.Empty);

        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateAsync(BanchanInstance instance)
    {   
        using var conn = _connectionFactory.CreateConnection();
    
        if (conn is not SqliteConnection sqliteConn)
        {
            throw new InvalidOperationException("이 저장소는 SQLite 연결만 지원합니다.");
        }

        await sqliteConn.OpenAsync();

        var command = sqliteConn.CreateCommand();
        command.CommandText =
        @"
            UPDATE BanchanInstance 
            SET Memo = @Memo,
                RemainingPortion = @RemainingPortion,
                UpdatedAt = @UpdatedAt,
                FinishedAt = @FinishedAt
            WHERE Id = @Id
        ";
        command.Parameters.AddWithValue("@Id", instance.Id);
        command.Parameters.AddWithValue("@Memo", instance.Memo ?? string.Empty);
        command.Parameters.AddWithValue("@RemainingPortion", instance.RemainingPortion);
        command.Parameters.AddWithValue("@UpdatedAt", instance.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@FinishedAt", instance.FinishedAt.HasValue ? instance.FinishedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : DBNull.Value);

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(string id)
    {
        using var conn = _connectionFactory.CreateConnection();
    
        if (conn is not SqliteConnection sqliteConn)
        {
            throw new InvalidOperationException("이 저장소는 SQLite 연결만 지원합니다.");
        }

        await sqliteConn.OpenAsync();

        var command = sqliteConn.CreateCommand();
        command.CommandText = 
        @"
            DELETE FROM BanchanInstance 
            WHERE Id = @Id
        ";
        command.Parameters.AddWithValue("@Id", id);

        await command.ExecuteNonQueryAsync();
    }
}