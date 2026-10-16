using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Dtos;

public record CreateGameDto(
    [Required] [StringLength(35)] string Name,
    [Required] [StringLength(20)]string Genre,
    [Required] [Range(0, 150.99)] decimal Price,
    [Required] DateOnly ReleaseDate
);
