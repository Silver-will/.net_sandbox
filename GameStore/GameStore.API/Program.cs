using GameStore.API.Data;
using GameStore.API.EndPoints;
using GameStore.API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

builder.AddGameStoreDb();

var app = builder.Build();

app.MapGamesEndPoints();

app.MigrateDb();

app.Run();
