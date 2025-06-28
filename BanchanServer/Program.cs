using BanchanServer.Database.ConnectionFactory;
using BanchanServer.Database.Repositories.Interfaces;
using BanchanServer.Database.Repositories.SQLite;
using BanchanServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IDbConnectionFactory, SQLiteConnectionFactory>();
builder.Services.AddScoped<IBanchanRepository, SQLiteBanchanRepository>();
builder.Services.AddScoped<IBanchanInstanceRepository, SQLiteBanchanInstanceRepository>();
builder.Services.AddScoped<BanchanService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options => {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
