namespace BanchanServer.Database.Initialization;

public static class DbInitializationExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddSingleton<IDatabaseInitializer, SQLiteInitializer>();

        return services;
    }

    public static IApplicationBuilder UseDatabase(this IApplicationBuilder app)
    {
        var databaseInitializer = app.ApplicationServices.GetRequiredService<IDatabaseInitializer>();
        
        databaseInitializer.Initialize();
        
        return app;
    }
}