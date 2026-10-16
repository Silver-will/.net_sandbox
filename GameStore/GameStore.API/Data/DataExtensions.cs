using GameStore.API.Data;
using GameStore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

        dbContext.Database.Migrate();
    }

    public static void AddGameStoreDb(this WebApplicationBuilder builder, string connectionString = "")
    {
        var connString = builder.Configuration.GetConnectionString("GameStore") ?? throw new InvalidOperationException("Connection string 'GameStore' not found.");
        builder.Services.AddSqlite<GameStoreContext>(
            connString,
            optionsAction: options => options.UseSeeding((context, _) =>
            {
                if (!context.Set<Genre>().Any())
                {
                    context.Set<Genre>().AddRange(
                        new Genre { Name = "Fighting" },
                        new Genre { Name = "Adventure" },
                        new Genre { Name = "Fighting" },
                        new Genre { Name = "Horror" },
                        new Genre { Name = "RPG" },
                        new Genre { Name = "Shooter" }
                    );

                    context.SaveChanges();
                }
            })
            );
    }
}
