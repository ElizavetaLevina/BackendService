using BackendService.API.Configurations;

namespace BackendService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            var isInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

            // Add services to the container.\
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
           
            ConfigureServicesDatabase.ConfigureServices(builder.Services, config, isInDocker);
            ConfigureServicesLogger.ConfigureServices(builder, isInDocker);
            ConfigureServicesAutoMapper.ConfigureServices(builder.Services);
            ConfigureServicesLogic.ConfigureServices(builder.Services, config);
            ConfigureServicesAuthorization.ConfigureServices(builder.Services, config, isInDocker);
            ConfigureServicesSwagger.ConfigureServices(builder.Services, config);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                ConfigureServicesSwagger.Configure(app, config);
            }

            app.UseExceptionHandler();

            app.UseMiddleware<LoggingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
