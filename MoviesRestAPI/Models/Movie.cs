using System.ComponentModel.DataAnnotations;

namespace MoviesRestAPI.Models;

public class Movie
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    public int ReleaseYear { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; }

    // Foreign Key
    public int GenreId { get; set; }

    // Navigation Property
    public Genre Genre { get; set; }
}