using Microsoft.EntityFrameworkCore;
using MoviesRestAPI.Models;
using MoviesRestAPI.Service;
using MoviesRestAPI.Service.Impl;

namespace MoviesRestAPI;

public class Program {

    public static void Main(String[] args) {
        var builder = WebApplication.CreateBuilder(args);
        
            
        if (builder.Environment.IsEnvironment("Testing"))
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));
        }
        else
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        }
            
        // Register the MovieService for Dependency Injection
        builder.Services.AddScoped<IMovieService, MovieServiceImpl>();
            
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
            
        var app = builder.Build();
            
        // Ensure the database is created when the application starts
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();  // This will create the tables based on your models
        }
        
        // Ensure the database is created and insert genres if none exist
        using (var scope = app.Services.CreateScope()) {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Check if genres exist, if not add them
            if (builder.Environment.IsEnvironment("Testing")) {
                dbContext.Genres.AddRange(
                    new Genre { Name = "Action" },
                    new Genre { Name = "Comedy" },
                    new Genre { Name = "Drama" },
                    new Genre { Name = "Horror" },
                    new Genre { Name = "Sci-Fi" },
                    new Genre { Name = "Romance" }
                );
                dbContext.SaveChanges();
            }

            dbContext.Database.EnsureCreated();
        }
            
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
        }
            
        app.UseHttpsRedirection();
            
        app.UseAuthorization();
            
        app.MapControllers();
            
        app.Run();
    }
    
   
}