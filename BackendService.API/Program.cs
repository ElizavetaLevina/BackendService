using AutoMapper;
using BackendService.BLL.Interfaces;
using BackendService.BLL.Logics;
using BackendService.DAL.Mappings;
using BackendService.DAL.Models;
using BackendService.DAL.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.OpenApi.Models;
using System.Reflection;
using DotNetEnv;

namespace BackendService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // Add services to the container.\
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BackendService.API", Version = "v1" });

                var xmlFail = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFail);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                c.EnableAnnotations();
            });

            builder.Services.AddDbContext<ApplicationDbContext>(
                option =>
                {
                    option.UseNpgsql(config.GetConnectionString(nameof(ApplicationDbContext)));
                });

            Env.Load();

            builder.Services.AddSingleton<INewsRepository, NewsRepository>(c =>
            {
                var apiKey = Environment.GetEnvironmentVariable("API_KEY");
                var baseUrl = Environment.GetEnvironmentVariable("BASE_API_URL");
                return new NewsRepository(apiKey, baseUrl);
            });

            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();

            builder.Services.AddScoped<IPostLogic, PostLogic>();
            builder.Services.AddScoped<ITagLogic, TagLogic>();
            builder.Services.AddScoped<IImageLogic, ImageLogic>();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<AppMappingProfile>();
            }, NullLoggerFactory.Instance);

            mapperConfig.AssertConfigurationIsValid();

            builder.Services.AddSingleton(mapperConfig.CreateMapper());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseExceptionHandler();

            app.MapControllers();

            app.Run();
        }
    }
}
