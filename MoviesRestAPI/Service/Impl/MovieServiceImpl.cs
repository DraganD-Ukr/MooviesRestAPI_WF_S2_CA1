using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesRestAPI.DTO;
using MoviesRestAPI.Models;

namespace MoviesRestAPI.Service.Impl;

public class MovieServiceImpl:IMovieService {
    
    private readonly ApplicationDbContext _context;

    public MovieServiceImpl(ApplicationDbContext context)
    {
        _context = context;
    }
    
    
    
    public async Task<List<MovieResponse>> GetAllMoviesAsync() {
        // Fetch movies with their Genre
        var movies = await _context.Movies
            .Include(m => m.Genre)
            .ToListAsync();

        // Project movies to MovieResponse
        var movieResponses = movies.Select(movie => new MovieResponse
            (
                movie.Id, 
                movie.Title, 
                movie.ReleaseYear, 
                movie.Description, 
                new GenreResponse(movie.GenreId, movie.Genre.Name))
            )
        .ToList();

        return movieResponses;
    }

    
    
    public async Task<MovieResponse> GetMovieByIdAsync(int id) {
        
        
        var movie = await _context.Movies
            .Include(m => m.Genre)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null)
            return null;

        var movieDto = new MovieResponse(
            movie.Id,
            movie.Title,
            movie.ReleaseYear,
            movie.Description,
            new GenreResponse(movie.GenreId, movie.Genre.Name)
        );  

        return movieDto;
    }

    
    
    public async Task<IActionResult> CreateMovieAsync(MovieRequest movieRequest) {
        
        Genre genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == movieRequest.Genre);
        
        if (genre == null) {
            return new BadRequestObjectResult(new { message = "Genre '"+movieRequest.Genre+"' not found" });
        }
        
        // Check if a same movie already exists
        bool movieExists = await _context.Movies
            .AnyAsync(m => m.Title == movieRequest.Title 
                           && m.GenreId == genre.Id 
                           && m.ReleaseYear == movieRequest.ReleaseYear);

        if (movieExists)
        {
            return new BadRequestObjectResult(new { message = "Movie with the title '" + movieRequest.Title 
                + "', release year '" + movieRequest.ReleaseYear 
                + "', and genre '" + movieRequest.Genre + "' already exists." });
        }
        
        var movie = new Movie
        {
            Title = movieRequest.Title,
            ReleaseYear = movieRequest.ReleaseYear,
            Description = movieRequest.Description,
            GenreId = genre.Id
        };

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        
        // Fetch the movie ID from the database to ensure it's saved
        var savedMovie = await _context.Movies
            .Where(m => m.Title == movieRequest.Title && m.ReleaseYear == movieRequest.ReleaseYear && m.GenreId == genre.Id)
            .OrderByDescending(m => m.Id) // In case of duplicates, get the latest one
            .FirstOrDefaultAsync();

        if (savedMovie == null)
        {
            return new ObjectResult(new {message="Failed to save movie"}) { StatusCode = StatusCodes.Status500InternalServerError };
        }
        
        var response = new MovieResponse(
            savedMovie.Id,
            savedMovie.Title,
            savedMovie.ReleaseYear,
            savedMovie.Description,
            new GenreResponse(savedMovie.GenreId, genre.Name)
        );

        return new CreatedAtActionResult("CreateMovie", "movies", new { id = savedMovie.Id }, response);
    }

    
    
    public async Task<IActionResult> UpdateMovieAsync(int id, MovieRequest updatedMovie) {
    
        // Check if the movie with the provided id exists
        var movie = await _context.Movies.Include(m => m.Genre).FirstOrDefaultAsync(m => m.Id == id);
    
        if (movie == null)
        {
            return new NotFoundObjectResult(new { message = "Movie not found with id: " + id });
        }

        // Find the genre by name
        Genre genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == updatedMovie.Genre);
    
        if (genre == null) {
            return new BadRequestObjectResult(new { message = "Genre '" + updatedMovie.Genre + "' not found" });
        }
    
        // Check if the movie with the same title, genre, and release year already exists (excluding the movie being updated)
        bool movieExists = await _context.Movies
            .AnyAsync(m => m.Title == updatedMovie.Title 
                           && m.GenreId == genre.Id 
                           && m.ReleaseYear == updatedMovie.ReleaseYear 
                           && m.Id != id);  // Make sure to exclude the current movie being updated

        if (movieExists)
        {
            return new BadRequestObjectResult(new { message = "Movie with the title '" + updatedMovie.Title 
                + "', release year '" + updatedMovie.ReleaseYear 
                + "', and genre '" + updatedMovie.Genre + "' already exists." });
        }
    
        // Update the movie's properties
        movie.Title = updatedMovie.Title;
        movie.ReleaseYear = updatedMovie.ReleaseYear;
        movie.Description = updatedMovie.Description;
        movie.GenreId = genre.Id;

        // Save the changes
        _context.Movies.Update(movie);  // Use Update instead of Add for modifying an existing entity
        await _context.SaveChangesAsync();

        // Return the updated movie as part of the response

        var movieResponse = new MovieResponse(
            movie.Id,
            movie.Title,
            movie.ReleaseYear,
            movie.Description,
            new GenreResponse(movie.GenreId, movie.Genre.Name)
        );
        
        return new OkObjectResult(movieResponse); 
    }


    
    
    public async Task<IActionResult> DeleteMovieAsync(int id) {
        
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null) return new NotFoundObjectResult(new { message = "Movie with id " + id + " does not exist" });

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return new StatusCodeResult(StatusCodes.Status204NoContent);
    }
}