namespace MoviesRestAPI.DTO;

public class GenreResponse {
    public int Id { get; set; }
    public string Name { get; set; }
    
    public GenreResponse(int id, string name)
    {
        Id = id;
        Name = name;
    }
}