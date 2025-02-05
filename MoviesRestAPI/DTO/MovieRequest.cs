using System.ComponentModel.DataAnnotations;

namespace MoviesRestAPI.DTO;

public class MovieRequest {
    
    [Required]
    [MaxLength(255)]
    public required string Title { get; set; }
    
    [Required]
    [Range(1888, 2025, ErrorMessage = "Release year must be between 1888 and 2025.")]
    public required int ReleaseYear { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public required string Description { get; set; }
    
    [Required]
    public required String Genre { get; set; }
}