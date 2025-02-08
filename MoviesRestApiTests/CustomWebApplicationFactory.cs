using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using MoviesRestAPI;

namespace MoviesRestApiTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public HttpClient GetClient()
        {
            // Override the ConfigureWebHost method to set the environment to "Testing"
            return WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                    {
                        // If you need to manually set configuration settings or override environment variables
                        var environment = "Testing"; // Or set it from an environment variable
                        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
                    })
                    .UseEnvironment("Testing"); // Explicitly set the environment here
            }).CreateClient();
        }
    }
}