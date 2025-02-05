namespace MoviesRestAPI.DTO
{
    public class MovieResponse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int ReleaseYear { get; set; }

        public string Description { get; set; }

        public GenreResponse Genre { get; set; }

        // Constructor to initialize all properties
        public MovieResponse(int id, string title, int releaseYear, string description, GenreResponse genre)
        {
            Id = id;
            Title = title;
            ReleaseYear = releaseYear;
            Description = description;
            Genre = genre;
        }
    }
}