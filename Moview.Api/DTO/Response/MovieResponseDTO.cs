namespace Moview.Api.DTO.Response;

public record MovieResponseDTO
{
    public int Id { get; init; } 
    public string Name { get; init; } 
    public string Description { get; init; }
    public string Genre { get; init; } 
    public string Language { get; init; } 
    public int Rating { get; init; }  
}

