using BanchanServer.Models.Entities;
using BanchanServer.Database.ConnectionFactory;
using BanchanServer.Database.Repositories.Interfaces;
using Microsoft.Data.Sqlite;

namespace BanchanServer.Database.Repositories.SQLite;

public class SQLiteBanchanRepository(IDbConnectionFactory connectionFactory) : IBanchanRepository
{
    private const int SQLITE_CONSTRAINT = 19;
    private readonly IDbConnectionFactory _connectionFactory = connectionFactory;

    public async Task<IEnumerable<Banchan>> GetAllAsync()
    {
        using var conn = _connectionFactory.CreateConnection();

        if (conn is not SqliteConnection sqliteConn)
        {
            throw new InvalidOperationException("이 저장소는 SQLite 연결만 지원합니다.");
        }

        await sqliteConn.OpenAsync();

        var command = sqliteConn.CreateCommand();
        command.CommandText = "SELECT * FROM Banchan";

        using var reader = await command.ExecuteReaderAsync();

        var banchans = new List<Banchan>();

        while (await reader.ReadAsync())
        {
            var banchan = new Banchan
            {
                Id = reader.GetString(0),
                Name = reader.GetString(1),
                CreatedAt = DateTime.Parse(reader.GetString(2)),
                UpdatedAt = DateTime.Parse(reader.GetString(3))
            };

            banchans.Add(banchan);
        }

        return banchans;
    }

    public async Task<Banchan?> GetByIdAsync(string id)
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
            SELECT * FROM Banchan 
            WHERE Id = @Id
        ";
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Banchan
            {
                Id = reader.GetString(0),
                Name = reader.GetString(1),
                CreatedAt = DateTime.Parse(reader.GetString(2)),
                UpdatedAt = DateTime.Parse(reader.GetString(3))
            };
        }
        return null;
    }
    
    public async Task AddAsync(Banchan banchan)
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
            INSERT INTO Banchan (Id, Name, CreatedAt) 
            VALUES (@Id, @Name, @CreatedAt)
        ";
        command.Parameters.AddWithValue("@Id", banchan.Id);
        command.Parameters.AddWithValue("@Name", banchan.Name);
        command.Parameters.AddWithValue("@CreatedAt", banchan.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));

        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch (SqliteException ex) when (ex.SqliteErrorCode == SQLITE_CONSTRAINT)
        {
            throw new InvalidOperationException($"반찬 이름 '{banchan.Name}'이(가) 이미 존재합니다.", ex);
        }
    }

    public async Task UpdateAsync(Banchan banchan)
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
            UPDATE Banchan 
            SET Name = @Name
            WHERE Id = @Id
        ";
        command.Parameters.AddWithValue("@Id", banchan.Id);
        command.Parameters.AddWithValue("@Name", banchan.Name);

        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch (SqliteException ex) when (ex.SqliteErrorCode == SQLITE_CONSTRAINT)
        {
            throw new InvalidOperationException($"반찬 이름 '{banchan.Name}'이(가) 이미 존재합니다.", ex);
        }
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
            DELETE FROM Banchan 
            WHERE Id = @Id
        ";
        command.Parameters.AddWithValue("@Id", id);

        await command.ExecuteNonQueryAsync();
    }
}