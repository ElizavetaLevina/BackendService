using BackendService.API;
using BackendService.DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace BackendService.Tests.Factories
{
	public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
	{
		private ApplicationDbContext? _dbContext;

		public ApplicationDbContext DbContext
		{
			get
			{
				return _dbContext ?? throw new InvalidOperationException("DbContext не инициализирован");
			}
		}

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.UseEnvironment("Test");
		}

		public Task InitializeAsync()
		{
			var scope = Services.CreateScope();
			_dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			return Task.CompletedTask;
		}

		async Task IAsyncLifetime.DisposeAsync()
		{
			if (_dbContext != null)
				await _dbContext.DisposeAsync();
		}

		public ApplicationDbContext GetDbContext()
		{
			return _dbContext ?? throw new InvalidOperationException("DbContext не инициализирован");
		}
	}
}
