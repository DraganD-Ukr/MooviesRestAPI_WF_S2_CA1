using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoviesRestAPI.Models;

public class Genre
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Name { get; set; }

    // (1 -> Many relationship)
    [JsonIgnore]
    public ICollection<Movie>? Movies { get; set; } = new List<Movie>(); 
}