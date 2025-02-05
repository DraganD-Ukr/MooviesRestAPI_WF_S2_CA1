using System.ComponentModel.DataAnnotations;

namespace MoviesRestAPI.Models;

public class Genre
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Name { get; set; }

    // (1 -> Many relationship)
    public required ICollection<Movie> Movies { get; set; }
}