using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoviesRestAPI;
using MoviesRestAPI.DTO;
using MoviesRestAPI.Models;


namespace MoviesRestApiTests;

using Xunit;



public class MovieRestApiIntegrationTests : IClassFixture<CustomWebApplicationFactory> {
    
    
    private readonly CustomWebApplicationFactory _factory;
    private const string BaseUrl = "/api/v1/movies";

    // xUnit will automatically inject the fixture into the constructor
    public MovieRestApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    // Now you can use GetClient() in each test
    private HttpClient GetClient()
    {
        return _factory.GetClient();
    }
    
    
        [Fact]
        public async Task PostMovie_ReturnsCreatedStatusCode()
        {
            var client = GetClient();
            var newMovie = new
            {
                Title = "Inception",
                Description = "A thief who enters the dreams of others to steal secrets from their subconscious.",
                Genre = "Sci-Fi",
                ReleaseYear = 2010
            };

            var response = await client.PostAsJsonAsync(BaseUrl, newMovie);

            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }
    
        [Fact]
        public async Task GetMovies_ReturnsOkStatusCode()
        {
            var client = GetClient();
            var response = await client.GetAsync(BaseUrl);

            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
        

        [Fact]
        public async Task PutMovie_ReturnsOkStatusCode()
        {
            var client = GetClient();
            var movieToUpdate = new
            {
                Title = "Updated Movie",
                Description = "A thief who enters the dreams of others to steal secrets from their subconscious.",
                Genre = "Drama",
                ReleaseYear = 2022
            };

            var response = await client.PutAsJsonAsync(BaseUrl+"/1", movieToUpdate);

            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteMovie_ReturnsNoContentStatusCode()
        {
            var client = GetClient();
            var movieToDelete = new
            {
                Title = "Movie to Delete",
                Description = "A thief who enters the dreams of others to steal secrets from their subconscious.",
                Genre = "Action",
                ReleaseYear = 2021
            };

            var postResponse = await client.PostAsJsonAsync(BaseUrl, movieToDelete);
            var createdMovie = await postResponse.Content.ReadFromJsonAsync<MovieResponse>();
            var movieId = createdMovie?.Id;

            Assert.NotNull(movieId);

            var response = await client.DeleteAsync(BaseUrl+"/"+movieId);

            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
    
}