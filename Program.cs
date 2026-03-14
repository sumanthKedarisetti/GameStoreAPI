using GameStore.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

var keycloakSection = builder.Configuration.GetSection("Keycloak");
var authority = keycloakSection["Authority"]
    ?? throw new InvalidOperationException("Missing Keycloak:Authority in configuration.");
var audience = keycloakSection["Audience"] ?? keycloakSection["ClientId"]
    ?? throw new InvalidOperationException("Missing Keycloak:Audience (or Keycloak:ClientId) in configuration.");
var requireHttpsMetadata = keycloakSection.GetValue<bool?>("RequireHttpsMetadata")
    ?? authority.StartsWith("https://", StringComparison.OrdinalIgnoreCase);

builder.Services.AddAuthentication(options=>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options=>
{
    options.Authority = authority;
    options.Audience = audience;
    options.RequireHttpsMetadata = requireHttpsMetadata;
});
builder.Services.AddAuthorization(options =>
{
    // Enforce authentication for all endpoints by default
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

List<GameDTO> games=[
    new GameDTO(1,"The Legend of Zelda: Breath of the Wild","Action-Adventure",59.99m,new DateTime(2017,3,3),"Nintendo","Nintendo"),
    new GameDTO(2,"God of War","Action-Adventure",49.99m,new DateTime(2018,4,20),"Santa Monica Studio","Sony Interactive Entertainment"),
    new GameDTO(3,"Red Dead Redemption 2","Action-Adventure",59.99m,new DateTime(2018,10,26),"Rockstar Games","Rockstar Games"),
    new GameDTO(4,"The Witcher 3: Wild Hunt","Action RPG",39.99m,new DateTime(2015,5,19),"CD Projekt Red","CD Projekt"),
    new GameDTO(5,"Minecraft","Sandbox",26.95m,new DateTime(2011,11,18),"Mojang Studios","Mojang Studios")];   

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

//Get all games

app.MapGet("/games", () => games).RequireAuthorization();
//Get game by id
app.MapGet("/games/{id}",(int id)=>games.Find(g=>g.Id==id)).WithName("GetGame").RequireAuthorization();    

//Create a New Game
app.MapPost("/games",(CreateGameDTO NewGame) =>
{
    GameDTO game=new GameDTO(games.Count+1,NewGame.Name,NewGame.Genre,NewGame.Price,DateTime.Now,NewGame.Developer,NewGame.Publisher);
    games.Add(game);  
}).RequireAuthorization();

app.MapPut("/games/{id}",(int id,CreateGameDTO UpdatedGame)=>
{
    var index = games.FindIndex(g=>g.Id==id);
    if(index == -1)
    {
        return Results.NotFound();
    }
    var updatedGame = games[index] with { Name = UpdatedGame.Name, Genre = UpdatedGame.Genre, Price = UpdatedGame.Price, Developer = UpdatedGame.Developer, Publisher = UpdatedGame.Publisher };
    games[index] = updatedGame;
    return Results.NoContent();
});

app.MapDelete("/games/{id}",(int id)=>
{
    var index = games.FindIndex(g=>g.Id==id);
    if(index == -1)
    {
        return Results.NotFound();
    }
    games.RemoveAt(index);
    return Results.Ok(games);
}).RequireAuthorization();

// default root endpoint
app.MapGet("/", () => "Game Store API is running. Use a Bearer token to access protected endpoints.").AllowAnonymous();

app.Run();
 