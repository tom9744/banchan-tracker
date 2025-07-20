using BanchanServer.Database.ConnectionFactory;
using BanchanServer.Database.Initialization;
using BanchanServer.Database.Repositories.Interfaces;
using BanchanServer.Database.Repositories.SQLite;
using BanchanServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDatabase();
builder.Services.AddOpenApi();

// CORS 설정 추가
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddScoped<IDbConnectionFactory, SQLiteConnectionFactory>();
builder.Services.AddScoped<IBanchanRepository, SQLiteBanchanRepository>();
builder.Services.AddScoped<IBanchanInstanceRepository, SQLiteBanchanInstanceRepository>();
builder.Services.AddScoped<IBanchanInstanceLogRepository, SQLiteBanchanInstanceLogRepository>();
builder.Services.AddScoped<BanchanService>();
builder.Services.AddScoped<BanchanInstanceService>();
builder.Services.AddScoped<BanchanInstanceLogService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options => {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseDatabase();

// CORS 미들웨어 추가
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
