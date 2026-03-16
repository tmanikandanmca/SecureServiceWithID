namespace Moview.Api.Model;

public class Movie
{
    public int Id { get; set; } 
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Language { get; set; } =  string.Empty;
    public int Rating { get; set; }  
}