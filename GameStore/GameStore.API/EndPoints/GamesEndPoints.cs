using System;
using GameStore.API.Data;
using GameStore.API.Dtos;
using GameStore.API.Models;

namespace GameStore.API.EndPoints;

public static class GamesEndPoints
{
    const string GetGameEndpointName = "GetGameById";
    private static readonly List<GameDtos> games = [
    new(1, "Street Fighte r 6", "Fighting", 69.99m, new DateOnly(2023, 2, 14)),
    new(2, "Elden Ring", "RPG", 59.99m, new DateOnly(2022, 2, 25)),
    new(3, "Hogwarts Legacy", "RPG", 59.99m, new DateOnly(2023, 2, 10)),
];

    public static void MapGamesEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");
        // GET /games
        group.MapGet("/", () => games);

        //GET /games/1
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        })
            .WithName(GetGameEndpointName);


        //POST/games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };
            /*var game = new GameDtos(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );*/
            dbContext.Games.Add(game);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        //PUT /games/id
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);
            if (index == -1)
            {
                //return Results.NotFound();
                var game = new GameDtos(
                games.Count + 1,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
                );
                games.Add(game);
                return Results.NoContent();
            }
            games[index] = new GameDtos(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });


        //DELETE /games/id
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });
    }
}
