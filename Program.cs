using System.Reflection.Metadata.Ecma335;
using GameStore.API.DTOs;

var builder = WebApplication.CreateBuilder(args);


List<GameDTO> games=[new GameDTO(1,"The Legend of Zelda: Breath of the Wild","Action-Adventure",59.99m,new DateTime(2017,3,3),"Nintendo","Nintendo"),
    new GameDTO(2,"God of War","Action-Adventure",49.99m,new DateTime(2018,4,20),"Santa Monica Studio","Sony Interactive Entertainment"),
    new GameDTO(3,"Red Dead Redemption 2","Action-Adventure",59.99m,new DateTime(2018,10,26),"Rockstar Games","Rockstar Games")];   

var app = builder.Build();


//Get all games
app.MapGet("/games", () => games);
//Get game by id
app.MapGet("/games/{id}",(int id)=>games.Find(g=>g.Id==id)).WithName("GetGame");    

//Create a New Game
app.MapPost("/games",(CreateGameDTO NewGame) =>
{
    GameDTO game=new GameDTO(games.Count+1,NewGame.Name,NewGame.Genre,NewGame.Price,DateTime.Now,NewGame.Developer,NewGame.Publisher);
    games.Add(game);  
});
app.Run();
 