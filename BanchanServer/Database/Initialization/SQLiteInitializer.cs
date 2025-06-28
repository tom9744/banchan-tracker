using Microsoft.Data.Sqlite;

namespace BanchanServer.Database.Initialization;

public class SQLiteInitializer(IConfiguration configuration, ILogger<SQLiteInitializer> logger) : IDatabaseInitializer
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("ConnectionString 항목이 없습니다.");
    private readonly string _schemaPath = configuration["Database:SchemaFile"]
        ?? throw new InvalidOperationException("SchemaFile 항목이 없습니다.");
    private readonly ILogger<SQLiteInitializer> _logger = logger;

    public void Initialize()
    {
        var builder = new SqliteConnectionStringBuilder(_connectionString);
        var targetDirectory = Path.GetDirectoryName(builder.DataSource);

        if (!string.IsNullOrEmpty(targetDirectory))
        {
            _logger.LogInformation("디렉토리 생성: {Directory}", targetDirectory);
            Directory.CreateDirectory(targetDirectory);
        }

        _logger.LogInformation("데이터베이스 연결: {ConnectionString}", _connectionString);
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = File.ReadAllText(_schemaPath);

        _logger.LogInformation("스키마 파일 실행: {SchemaPath}", _schemaPath);
        var result = command.ExecuteNonQuery();

        _logger.LogInformation("스키마 파일 실행 결과: {SchemaPath}, 변경된 행 수: {Result}", _schemaPath, result);
    }
}