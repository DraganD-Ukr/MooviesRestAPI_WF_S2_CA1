using System.ComponentModel.DataAnnotations;

namespace MoviesRestAPI.Models;

public class Genre
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    // (1 -> Many relationship)
    public ICollection<Movie> Movies { get; set; }
}