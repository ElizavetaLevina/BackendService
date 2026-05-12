using BackendService.API;
using BackendService.DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BackendService.Tests.Factories
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public ApplicationDbContext _dbContext;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("API_KEY", "test");
            Environment.SetEnvironmentVariable("BASE_API_URL", "http://test");

            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql("Host=localhost;Port=5430;Database=backend_service_tests;Username=postgres;Password=postgres"));
            });
        }

        public Task InitializeAsync()
        {
            var scope = Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return Task.CompletedTask;
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
    }
}
