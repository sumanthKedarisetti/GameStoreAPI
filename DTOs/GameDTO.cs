
namespace GameStore.API.DTOs;
public record GameDTO(int Id,String Name,String Genre, decimal Price,
    DateTime ReleaseDate, String Developer, String Publisher);
