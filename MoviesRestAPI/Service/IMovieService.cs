using Microsoft.AspNetCore.Mvc;
using MoviesRestAPI.DTO;
using MoviesRestAPI.Models;

namespace MoviesRestAPI.Service;

public interface IMovieService {
    
    Task<List<MovieResponse>> GetAllMoviesAsync();
    Task<MovieResponse> GetMovieByIdAsync(int id);
    Task<IActionResult> CreateMovieAsync(MovieRequest movie);
    Task<IActionResult> UpdateMovieAsync(int id, MovieRequest updatedMovie);
    Task<IActionResult> DeleteMovieAsync(int id);
    
}