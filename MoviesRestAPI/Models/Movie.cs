using System.ComponentModel.DataAnnotations;

namespace MoviesRestAPI.Models;

public class Movie
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Title { get; set; }

    
    [Range(1888, 2025, ErrorMessage = "Release year must be between 1888 and 2025.")]
    [Required]
    public required int ReleaseYear { get; set; }

    [MaxLength(1000)]
    public required string Description { get; set; }

    // Foreign Key
    public int GenreId { get; set; }

    // Navigation Property
    public required Genre Genre { get; set; }
}