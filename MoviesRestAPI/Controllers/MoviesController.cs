using Microsoft.AspNetCore.Mvc;
using MoviesRestAPI.DTO;
using MoviesRestAPI.Models;
using MoviesRestAPI.Service;

namespace MoviesRestAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class MoviesController : ControllerBase {
    
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    
    
    [HttpGet]
    public async Task<IActionResult> GetMovies()
    {
        var movies = await _movieService.GetAllMoviesAsync();
        return Ok(movies);
    }

    [HttpGet()]
    [Route("{id:int}")]
    public async Task<IActionResult> GetMovie([FromRoute] int id)
    {

        if (id <= 0) {
            return BadRequest(new { message = "Invalid movie id" });
        }
        
        var movie = await _movieService.GetMovieByIdAsync(id);
        if (movie == null) return NotFound(new { message = "Movie not found with id: "+id });
        return Ok(movie);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovie([FromBody] MovieRequest movieRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Return validation errors if any
        }

        // Call service layer to handle the movie creation
        var result = await _movieService.CreateMovieAsync(movieRequest);
        

        return result;
    }

    [HttpPut()]
    [Route("{id:int}")]
    public async Task<ActionResult> UpdateMovie([FromRoute] int id, [FromBody] MovieRequest movieRequest)
    {
        if (id <= 0) {
            return BadRequest(new { message = "Invalid movie id" });
        }
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Return validation errors if any
        }
        
        var result = await _movieService.UpdateMovieAsync(id, movieRequest);
    
        return Ok(result);  // If movie was successfully updated, return the updated movie
    }

    [HttpDelete("{id}")]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteMovie([FromRoute] int id)
    {
        if (id <= 0) {
            return BadRequest(new { message = "Invalid movie id" });
        }
        
        var deleted = await _movieService.DeleteMovieAsync(id);
        return deleted;
    }
    
}